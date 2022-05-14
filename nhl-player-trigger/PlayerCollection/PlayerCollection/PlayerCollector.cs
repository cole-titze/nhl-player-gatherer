using DataAccess.Models;
using DataAccess.Repository;
using Microsoft.Extensions.Logging;

namespace PlayerCollection.PlayerCollection
{
    public class PlayerCollector
    {
        private ILogger _logger;
        private readonly IPlayerRepository _repo;
        public PlayerCollector(ILogger logger, IPlayerRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }
        public async Task GetPlayers()
        {
            
        }
    }
}
