using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ildar.Wallet.Bot.DataAccess;

public static class CompositionRoot
{
    public static IServiceCollection ConfigureDataAccessServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<BotDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("BloggingContext")));

        return services;
    }
}