using DataAccess.Models;

namespace PlayerCollection.RequestMaker
{
    public interface IRosterRequestMaker
    {
        Task<List<IPlayerStats>> GetPlayerIds(int year);
    }
}
