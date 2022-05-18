using DataAccess.Models;

namespace PlayerCollection.RequestMaker
{
    public interface IPlayerRequestMaker
    {
        Task<List<PlayerStats>> GetPlayersByIds(List<PlayerStats> ids, int year);
    }
}
