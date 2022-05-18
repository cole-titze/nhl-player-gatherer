using DataAccess.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace PlayerCollection.RequestMaker
{
    public class RosterRequestMaker : IRosterRequestMaker
    {
        private const string _url = "http://statsapi.web.nhl.com/api/v1/teams/";
        private const string _query = "/roster?season=";
        public async Task<List<PlayerStats>> GetPlayerIds(int year)
        {
            var players = await GetPlayersFromRosters(year);
            return players;
        }
        private async Task<List<PlayerStats>> GetPlayersFromRosters(int year)
        {
            var players = new List<PlayerStats>();
            for(int teamId = 0; teamId < 60; teamId++)
            {
                var yearStr = GetYearString(year);
                var response = await MakeRosterRequest(_url + teamId.ToString() + _query + yearStr);
                if (!response.IsSuccessStatusCode)
                    continue;
                var teamPlayers = await BuildPlayers(response);
                if(teamPlayers.Count > 0)
                    players.AddRange(teamPlayers);
            }
            return players;
        }

        private string GetYearString(int year)
        {
            var futureYear = year + 1;
            return year.ToString() + futureYear.ToString();
        }

        public async Task<HttpResponseMessage> MakeRosterRequest(string query)
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
        private async Task<List<PlayerStats>> BuildPlayers(HttpResponseMessage response)
        {
            // Get data as Json string 
            string data = await response.Content.ReadAsStringAsync();
            // Add Json string conversion to hard object
            var message = JsonConvert.DeserializeObject<dynamic>(data);
            if (InvalidTeam(message))
                return new List<PlayerStats>();

            var players = ParseMessageToPlayers(message);
            return players;
        }
        private List<PlayerStats> ParseMessageToPlayers(dynamic message)
        {
            var players = new List<PlayerStats>();
            var roster = message.roster;
            foreach(var player in roster)
            {
                if (player.position.code == "G")
                    continue;
                var mappedPlayer = new PlayerStats()
                {
                    name = player.person.fullName,
                    id = player.person.id,
                    position = GetPosition(player),
                };
                if (mappedPlayer.position == POSITION.Goalie)
                    continue;
                players.Add(mappedPlayer);
            }
            return players;
        }

        private POSITION GetPosition(dynamic player)
        {
            string position = player.position.code;
            POSITION playerPosition;
            switch (position)
            {
                case "G":
                    playerPosition = POSITION.Goalie;
                    break;

                case "L":
                    playerPosition = POSITION.LeftWing;
                    break;

                case "R":
                    playerPosition = POSITION.RightWing;
                    break;

                case "D":
                    playerPosition = POSITION.Defenseman;
                    break;

                default:
                    playerPosition = POSITION.LeftWing;
                    break;
            }

            return playerPosition;
        }

        private bool InvalidTeam(dynamic message)
        {
            if (message.message == "Object not found")
                return true;


            return false;
        }
    }
}
