using DataAccess.Models;

namespace PlayerCollection.PlayerValueCollection
{
    public interface IPlayerValueCalculator
    {
        List<PlayerValue> GetPlayerValues(List<PlayerStats> players);
    }
}
