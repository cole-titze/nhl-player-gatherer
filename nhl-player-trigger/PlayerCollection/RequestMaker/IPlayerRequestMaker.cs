using DataAccess.Models;

namespace PlayerCollection.RequestMaker
{
    public interface IPlayerRequestMaker
    {
        Task<List<IPlayerStats>> GetPlayersByIds(List<IPlayerStats> ids, int year);
    }
}
