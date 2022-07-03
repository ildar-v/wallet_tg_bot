using Hello.Ildar.Bot.AppServices;
using Hello.Ildar.Bot.AppServices.Bot;
using Hello.Ildar.Bot.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hello.Ildar.Bot;

public static class CompositionRoot
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IBotService, BotService>();
        services.AddTransient<IBotDbService, BotDbService>();

        services.ConfigureDataAccessServices(configuration);
        
        return services;
    }
}