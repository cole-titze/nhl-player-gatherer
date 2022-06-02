using DataAccess.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace PlayerCollection.RequestMaker
{
    public class RosterRequestMaker : IRosterRequestMaker
    {
        private const string _url = "http://statsapi.web.nhl.com/api/v1/teams/";
        private const string _query = "/roster?season=";
        public async Task<List<IPlayerStats>> GetPlayerIds(int year)
        {
            var players = await GetPlayersFromRosters(year);
            return players;
        }
        private async Task<List<IPlayerStats>> GetPlayersFromRosters(int year)
        {
            var players = new List<IPlayerStats>();
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
        private async Task<List<IPlayerStats>> BuildPlayers(HttpResponseMessage response)
        {
            // Get data as Json string 
            string data = await response.Content.ReadAsStringAsync();
            // Add Json string conversion to hard object
            var message = JsonConvert.DeserializeObject<dynamic>(data);
            if (InvalidTeam(message))
                return new List<IPlayerStats>();

            var players = ParseMessageToPlayers(message);
            return players;
        }
        private List<IPlayerStats> ParseMessageToPlayers(dynamic message)
        {
            var players = new List<IPlayerStats>();
            var roster = message.roster;
            foreach(var player in roster)
            {
                IPlayerStats mappedPlayer;
                string positionCode = player.position.code;
                if (positionCode == "G")
                    mappedPlayer = new GoalieStats()
                    {
                        name = player.person.fullName,
                        id = player.person.id,
                        position = Mapper.StringPositionToPosition(positionCode),
                    };
                else
                {
                    mappedPlayer = new PlayerStats()
                    {
                        name = player.person.fullName,
                        id = player.person.id,
                        position = Mapper.StringPositionToPosition(positionCode),
                    };
                }

                players.Add(mappedPlayer);
            }
            return players;
        }

        private bool InvalidTeam(dynamic message)
        {
            if (message.message == "Object not found")
                return true;


            return false;
        }
    }
}
