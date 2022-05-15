using DataAccess.Models;

namespace PlayerCollection.PlayerValueCollection
{
    public class PlayerValueCalculator : IPlayerValueCalculator
    {
        public List<PlayerValue> GetPlayerValues(List<PlayerStats> players)
        {
            var playerValues = new List<PlayerValue>();
            foreach (var player in players)
            {
                playerValues.Add(new PlayerValue()
                {
                    name = player.name,
                    id = player.id,
                    value = GetSinglePlayerValue(player)
                });
            }
            return playerValues;
        }
        // Player Game Score = ( (0.75 * G) + (.63A) + (0.075 * SOG) + (0.05 * BLK) – (0.075 * PIM) + (0.15 * PM) + ((17*GP*FOP)*.01) ) / GP
        private double GetSinglePlayerValue(PlayerStats player)
        {
            if (player.gamesPlayed == 0)
                return 0;
            return ((.75 * player.goals) + (.63 * player.assists) + (.075 * player.shotsOnGoal) + (.05 * player.blockedShots) - (.075 * player.penaltyMinutes) 
                + (.15 * player.plusMinus) + ((17*player.gamesPlayed * player.faceoffPercent) * .01)) / player.gamesPlayed;
        }
    }
}
