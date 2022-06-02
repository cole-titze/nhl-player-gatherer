using DataAccess.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace PlayerCollection.RequestMaker
{
    public class PlayerRequestMaker : IPlayerRequestMaker
    {
        private const string _url = "http://statsapi.web.nhl.com/api/v1/people/";
        private const string _query = "/stats?stats=statsSingleSeason&season=";
        public async Task<List<IPlayerStats>> GetPlayersByIds(List<IPlayerStats> players, int year)
        {
            var yearStr = GetYearString(year);
            var mappedPlayers = new List<IPlayerStats>();
            for(int i = 0; i < players.Count; i++)
            {
                var player = players[i];
                var response = await MakePlayerRequest(_url + player.id.ToString() + _query + yearStr);
                if (!response.IsSuccessStatusCode)
                    continue;
                var playerStats = await BuildPlayer(response);
                var mappedPlayer = MapPlayer(player, playerStats);
                if(mappedPlayer.id != 0)
                    mappedPlayers.Add(mappedPlayer);
            }
            
            return mappedPlayers;
        }
        private string GetYearString(int year)
        {
            var futureYear = year + 1;
            return year.ToString() + futureYear.ToString();
        }
        private IPlayerStats MapPlayer(IPlayerStats player, IPlayerStats playerStats)
        {
            playerStats.id = player.id;
            playerStats.name = player.name;
            playerStats.position = player.position;

            return playerStats;
        }
        private async Task<IPlayerStats> BuildPlayer(HttpResponseMessage response)
        {
            // Get data as Json string 
            string data = await response.Content.ReadAsStringAsync();
            // Add Json string conversion to hard object
            var message = JsonConvert.DeserializeObject<dynamic>(data);
            if (InvalidPlayer(message))
                return new PlayerStats();

            var player = ParseMessageToPlayerStats(message);
            return player;
        }
        private bool InvalidPlayer(dynamic message)
        {
            if (message == null || message.message == "Internal error occurred")
                return true;
            return false;
        }
        private IPlayerStats ParseMessageToPlayerStats(dynamic message)
        {
            if (message.stats[0].splits.Count == 0)
                return new PlayerStats();
            var rawPlayer = message.stats[0].splits[0].stat;

            if(rawPlayer.faceOffPct == null)
                return new GoalieStats()
                {
                    goalsAgainst = rawPlayer.goalsAgainst,
                    saves = rawPlayer.saves,
                    gamesStarted = rawPlayer.gamesStarted,
                };

            return new PlayerStats()
            {
                gamesPlayed = rawPlayer.games,
                faceoffPercent = (rawPlayer.faceOffPct / 100),
                plusMinus = rawPlayer.plusMinus,
                penaltyMinutes = rawPlayer.pim,
                blockedShots = rawPlayer.blocked,
                shotsOnGoal = rawPlayer.shots,
                assists = rawPlayer.assists,
                goals = rawPlayer.goals,
            };
        }
        private async Task<HttpResponseMessage> MakePlayerRequest(string query)
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //GET Method
                response = await client.GetAsync(query);
            }
            return response;
        }
    }
}
