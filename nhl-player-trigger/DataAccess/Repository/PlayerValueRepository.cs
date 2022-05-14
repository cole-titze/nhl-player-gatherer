using DataAccess.Models;

namespace DataAccess.Repository
{
    public class PlayerValueRepository : IPlayerRepository
    {
        private readonly PlayerDbContext _dbContext;
        public PlayerValueRepository(PlayerDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
