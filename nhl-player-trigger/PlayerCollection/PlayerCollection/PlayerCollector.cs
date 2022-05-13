using Microsoft.Extensions.Logging;

namespace PlayerCollection.PlayerCollection
{
    public class PlayerCollector
    {
        private ILogger _logger;
        public PlayerCollector(ILogger logger)
        {
            _logger = logger;
        }
        public async Task GetPlayers()
        {

        }
    }
}
