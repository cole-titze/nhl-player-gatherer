namespace DataAccess.Models
{
    public class PlayerStats
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public int goals { get; set; }
        public int assists { get; set; }
        public int shotsOnGoal { get; set; }
        public int blockedShots { get; set; }
        public double penaltyMinutes { get; set; }
        public int plusMinus { get; set; }
        public double faceoffPercent { get; set; }
        public int gamesPlayed { get; set; }
        // Default to stop null warning left-wing has no special rules
        public POSITION position { get; set; } = POSITION.LeftWing;
    }
    public enum POSITION
    {
        Center,
        RightWing,
        LeftWing,
        Defenseman,
        Goalie
    }
}
