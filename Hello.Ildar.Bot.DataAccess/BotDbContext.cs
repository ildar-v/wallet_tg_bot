using Hello.Ildar.Bot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hello.Ildar.Bot.DataAccess;

public sealed class BotDbContext : DbContext
{
    // public DbSet<User> Users { get; set; }
    //
    // public DbSet<Record> Records { get; set; }

    public BotDbContext()
    {
    }

    public BotDbContext(DbContextOptions<BotDbContext> options)
        : base(options)
    {
        Database.Migrate();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql();

    public DbSet<User> Users { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Record> Records { get; set; }
}