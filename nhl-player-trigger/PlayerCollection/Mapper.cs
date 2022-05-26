using DataAccess.Models;

namespace PlayerCollection
{
	public static class Mapper
	{
        public static POSITION StringPositionToPosition(string position)
        {
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

                case "C":
                    playerPosition = POSITION.Center;
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
        public static string PlayerPositionToString(POSITION position)
        {
            string positionStr;
            switch (position)
            {
                case POSITION.Goalie:
                    positionStr = "G";
                    break;

                case POSITION.LeftWing:
                    positionStr = "L";
                    break;

                case POSITION.RightWing:
                    positionStr = "R";
                    break;

                case POSITION.Center:
                    positionStr = "C";
                    break;

                case POSITION.Defenseman:
                    positionStr = "D";
                    break;

                default:
                    positionStr = "L";
                    break;
            }

            return positionStr;
        }
    }
}

