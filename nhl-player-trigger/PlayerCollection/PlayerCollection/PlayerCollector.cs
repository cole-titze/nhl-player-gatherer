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
            var ids = await _rosterRequestMaker.GetPlayerIds();
            var players = await _playerRequestMaker.GetPlayersByIds(ids);
            var playersWithValues = _playerValueCalculator.GetPlayerValues(players);
            //await _repo.AddUpdatePlayers(playersWithValues);
        }
    }
}
