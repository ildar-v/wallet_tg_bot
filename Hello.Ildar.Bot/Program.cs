﻿using System.Reflection;
using Hello.Ildar.Bot;
using Hello.Ildar.Bot.AppServices.Bot;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

const string StopCommand = "stop";

var executingAssemblyName = Assembly.GetExecutingAssembly().GetName();
Console.WriteLine($"Starting application {executingAssemblyName.Name} {executingAssemblyName.Version}");

var configurationRoot = Conf();

IConfigurationRoot Conf()
{
    var builder = new ConfigurationBuilder();

    // add static values
    builder.AddInMemoryCollection(new Dictionary<string, string>
    {
        { "MyKey", "MyValue" },
    });

    // add values from a json file
    builder.AddJsonFile("appsettings.json");

    // create the IConfigurationRoot instance
    return builder.Build();

    // var value = config["MyKey"]; // get a value
    // var section = config.GetSection("SubSection"); //get a sectio
}

var serviceProvider = new ServiceCollection()
    .ConfigureServices(configurationRoot)
    .BuildServiceProvider();

var cancellationTokenSource = new CancellationTokenSource();

var botService = serviceProvider.GetRequiredService<IBotService>();
await botService.StartBot(cancellationTokenSource.Token);

while (true)
{
    var command = Console.ReadLine();

    if (command?.ToLower().Trim() == StopCommand)
    {
        return 0;
    }
}