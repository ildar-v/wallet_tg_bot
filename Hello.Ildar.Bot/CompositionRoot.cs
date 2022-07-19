using Hello.Ildar.Bot.AppServices.Bot;
using Hello.Ildar.Bot.AppServices.Data;
using Hello.Ildar.Bot.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hello.Ildar.Bot;

public static class CompositionRoot
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration);
        services.AddTransient<IBotService, BotService>();
        services.AddTransient<IBotAnswerService, BotAnswerService>();
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<ITelegramUserService, TelegramUserService>();
        services.AddTransient<IRecordService, RecordService>();

        services.ConfigureDataAccessServices(configuration);
        
        return services;
    }
}