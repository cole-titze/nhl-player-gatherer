﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NhlPlayerTrigger;

var collector = new NhlPlayerMain();

// Build service collection
var collection = new ServiceCollection();
collection.AddLogging(b => {
    b.SetMinimumLevel(LogLevel.Information);
});
var sp = collection.BuildServiceProvider();

// Get logger and run main
using (var scope = sp.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    string connectionString = config.GetConnectionString("GamesDatabase");
    await collector.Main(logger, connectionString);
}