using DataAccess.Models;
using DataAccess.Repository;
using Microsoft.Extensions.Logging;
using PlayerCollection.PlayerValueCollection;
using PlayerCollection.RequestMaker;

namespace PlayerCollection.PlayerCollection
{
    public class PlayerCollector
    {
        private ILogger _logger;
        private readonly IPlayerRepository _repo;
        private IRosterRequestMaker _rosterRequestMaker;
        private IPlayerRequestMaker _playerRequestMaker;
        private IPlayerValueCalculator _playerValueCalculator;
        private const int _cutOffCount = 300;

        public PlayerCollector(ILogger logger, IPlayerRepository repo, IRosterRequestMaker rosterRequestMaker, IPlayerRequestMaker playerRequestMaker, IPlayerValueCalculator playerValueCalculator)
        {
            _playerValueCalculator = playerValueCalculator;
            _logger = logger;
            _repo = repo;
            _rosterRequestMaker = rosterRequestMaker;
            _playerRequestMaker = playerRequestMaker;
        }
        public async Task GetAndStorePlayers()
        {
            // Should be moved up a level
            var dateRange = new DateRange()
            {
                StartYear = 2012,
                EndYear = GetEndSeason(DateTime.UtcNow),
            };

            for(int year = dateRange.StartYear; year <= dateRange.EndYear; year++)
            {
                var playerCount = await _repo.GetPlayerCountBySeason(year);
                // Skip if data is already in db and not the current year
                // If current year data could be incomplete so run anyways
                if (playerCount > _cutOffCount && year < dateRange.EndYear)
                    continue;
                var ids = await _rosterRequestMaker.GetPlayerIds(year);
                var players = await _playerRequestMaker.GetPlayersByIds(ids, year);
                var playersWithValues = _playerValueCalculator.GetPlayerValues(players);
                var distinctPlayers = RemoveDuplicates(playersWithValues);
                var playersWithYears = AddYears(distinctPlayers, year);
                await _repo.AddUpdatePlayers(playersWithYears);
            }
        }

        private List<PlayerValue> RemoveDuplicates(List<PlayerValue> playersWithValues)
        {
            return playersWithValues.GroupBy(x => x.id).Select(x => x.First()).ToList();
        }

        private List<PlayerValue> AddYears(List<PlayerValue> playersWithValues, int year)
        {
            foreach(var player in playersWithValues)
            {
                player.startYear = year;
            }
            return playersWithValues;
        }

        // Season spans 2 years (2021-2022) but we only want the start year of the season
        // (ex. February 2022 we want 2021 to be the end season)
        public int GetEndSeason(DateTime currentDate)
        {
            var endSeasonDate = new DateTime(currentDate.Year, 09, 15);
            int currentSeasonYear;

            if (currentDate > endSeasonDate)
                currentSeasonYear = currentDate.Year;
            else
                currentSeasonYear = currentDate.Year - 1;

            return currentSeasonYear;
        }
    }
}
