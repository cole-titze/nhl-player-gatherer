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

        public async Task AddUpdatePlayers(List<PlayerValue> playersWithValues)
        {
            await _dbContext.PlayerValue.AddRangeAsync(playersWithValues);
            _dbContext.SaveChanges();
        }
    }
}
