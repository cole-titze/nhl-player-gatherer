using DataAccess.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace PlayerCollection.RequestMaker
{
    public class PlayerRequestMaker : IPlayerRequestMaker
    {
        private const string _url = "http://statsapi.web.nhl.com/api/v1/people/";
        private const string _query = "/stats?stats=statsSingleSeason&season=20212022";
        public async Task<List<PlayerStats>> GetPlayersByIds(List<PlayerStats> players)
        {
            var mappedPlayers = new List<PlayerStats>();
            for(int i = 0; i < players.Count; i++)
            {
                var player = players[i];
                var response = await MakePlayerRequest(_url + player.id.ToString() + _query);
                if (!response.IsSuccessStatusCode)
                    continue;
                var playerStats = await BuildPlayer(response);
                var mappedPlayer = MapPlayer(player, playerStats);
                if(mappedPlayer.id != 0)
                    mappedPlayers.Add(mappedPlayer);
            }
            
            return mappedPlayers;
        }
        private PlayerStats MapPlayer(PlayerStats player, PlayerStats playerStats)
        {
            return new PlayerStats()
            {
                id = player.id,
                name = player.name,
                gamesPlayed = playerStats.gamesPlayed,
                faceoffPercent = playerStats.faceoffPercent,
                plusMinus = playerStats.plusMinus,
                penaltyMinutes = playerStats.penaltyMinutes,
                blockedShots = playerStats.blockedShots,
                shotsOnGoal = playerStats.shotsOnGoal,
                assists = playerStats.assists,
                goals = playerStats.goals,
            };
        }
        private async Task<PlayerStats> BuildPlayer(HttpResponseMessage response)
        {
            // Get data as Json string 
            string data = await response.Content.ReadAsStringAsync();
            // Add Json string conversion to hard object
            var message = JsonConvert.DeserializeObject<dynamic>(data);
            if (InvalidPlayer(message))
                return new PlayerStats();

            var players = ParseMessageToPlayerStats(message);
            return players;
        }
        private bool InvalidPlayer(dynamic message)
        {
            if (message == null || message.message == "Internal error occurred")
                return true;
            return false;
        }
        private PlayerStats ParseMessageToPlayerStats(dynamic message)
        {
            if (message.stats[0].splits.Count == 0)
                return new PlayerStats();
            var rawPlayer = message.stats[0].splits[0].stat;

            return new PlayerStats()
            {
                gamesPlayed = rawPlayer.games,
                faceoffPercent = rawPlayer.faceOffPct,
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
