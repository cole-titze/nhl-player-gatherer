using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

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
            var addList = new List<PlayerValue>();
            var updateList = new List<PlayerValue>();
            foreach(var player in playersWithValues)
            {
                var dbPlayer = _dbContext.PlayerValue.FirstOrDefault(p => p.id == player.id  && p.startYear == player.startYear);
                if (dbPlayer == null)
                    addList.Add(player);
                else
                {
                    dbPlayer.value = player.value;
                    dbPlayer.position = player.position;
                    updateList.Add(dbPlayer);
                }
            }
            _dbContext.PlayerValue.AddRange(addList);
            _dbContext.PlayerValue.UpdateRange(updateList);
            // Save to database
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> GetPlayerCountBySeason(int year)
        {
            return await _dbContext.PlayerValue.Where(i => i.startYear == year).CountAsync();
        }
    }
}
