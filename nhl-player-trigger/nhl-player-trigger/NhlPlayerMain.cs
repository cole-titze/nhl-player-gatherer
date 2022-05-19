using System;
using System.Threading.Tasks;
using DataAccess.Models;
using DataAccess.Repository;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using PlayerCollection.PlayerCollection;
using PlayerCollection.PlayerValueCollection;
using PlayerCollection.RequestMaker;

namespace NhlPlayerTrigger
{
    public class NhlPlayerMain
    {
        [FunctionName("Function1")]
        public async Task Run([TimerTrigger("0 0 4 * * *")]TimerInfo myTimer, ILogger logger)
        {
            string connectionString = System.Environment.GetEnvironmentVariable("GamesDatabase", EnvironmentVariableTarget.Process);
            await Main(logger, connectionString);
        }
        public async Task Main(ILogger logger, string connectionString)
        {
            // Run Data Collection
            logger.LogInformation("Starting Data Collection");
            var dbContext = new PlayerDbContext(connectionString);
            var playerRepository = new PlayerValueRepository(dbContext);
            var rosterRequestMaker = new RosterRequestMaker();
            var playerRequestMaker = new PlayerRequestMaker();
            var playerValueCalculator = new PlayerValueCalculator();

            var playerCollector = new PlayerCollector(logger, playerRepository, rosterRequestMaker, playerRequestMaker, playerValueCalculator);
            await playerCollector.GetAndStorePlayers();
            logger.LogInformation("Completed Data Collection");

            // Run Data Cleaning
            logger.LogInformation("Starting Data Cleaning");
            //var dataCleaner = new PlayerValueGetter(logger, gamesDA, futureGamesDA, cleanGamesDA, futureCleanedGamesDA, dateRange);
            //dataCleaner.CleanData();
            logger.LogInformation("Completed Data Cleaning");
        }

    }
}
