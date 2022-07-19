using Ildar.Wallet.Bot.AppServices.Bot;
using Ildar.Wallet.Bot.AppServices.Data;
using Ildar.Wallet.Bot.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ildar.Wallet.Bot;

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