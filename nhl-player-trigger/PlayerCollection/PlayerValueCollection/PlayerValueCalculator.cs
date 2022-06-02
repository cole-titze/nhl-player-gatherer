using DataAccess.Models;

namespace PlayerCollection.PlayerValueCollection
{
    public class PlayerValueCalculator : IPlayerValueCalculator
    {
        public List<PlayerValue> GetPlayerValues(List<IPlayerStats> players)
        {
            var playerValues = new List<PlayerValue>();
            foreach (var player in players)
            {
                double value;
                if (player.position == POSITION.Goalie)
                    value = player.GetPlayerValue();
                else
                    value = player.GetPlayerValue();
                playerValues.Add(new PlayerValue()
                {
                    name = player.name,
                    id = player.id,
                    value = value,
                    position = Mapper.PlayerPositionToString(player.position)
            });
            }
            return playerValues;
        }
    }
}
