using DataAccess.Models;

namespace PlayerCollection.RequestMaker
{
    public interface IRosterRequestMaker
    {
        Task<List<PlayerStats>> GetPlayerIds(int year);
    }
}
