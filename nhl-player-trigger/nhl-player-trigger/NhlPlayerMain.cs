using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace NhlPlayerTrigger
{
    public class NhlPlayerMain
    {
        [FunctionName("Function1")]
        public async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger logger)
        {
            string connectionString = System.Environment.GetEnvironmentVariable("GamesDatabase", EnvironmentVariableTarget.Process);
            await Main(logger, connectionString);
        }
        public async Task Main(ILogger logger, string connectionString)
        {
            // Run Data Collection
            logger.LogInformation("Starting Data Collection");


            var dataGetter = new PlayerGetter(gameParser, scheduleParser, scheduleRequestMaker, gameRequestMaker, gamesDA, futureGamesDA, predictedGamesDA, dateRange, logger);
            await dataGetter.GetData();
            logger.LogInformation("Completed Data Collection");

            // Run Data Cleaning
            logger.LogInformation("Starting Data Cleaning");
            var dataCleaner = new PlayerValueGetter(logger, gamesDA, futureGamesDA, cleanGamesDA, futureCleanedGamesDA, dateRange);
            dataCleaner.CleanData();
            logger.LogInformation("Completed Data Cleaning");
        }

    }
}
