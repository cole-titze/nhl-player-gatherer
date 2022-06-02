using DataAccess.Models;

namespace PlayerCollection.PlayerValueCollection
{
    public interface IPlayerValueCalculator
    {
        List<PlayerValue> GetPlayerValues(List<IPlayerStats> players);
    }
}
