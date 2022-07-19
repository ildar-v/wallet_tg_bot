using Ildar.Wallet.Bot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ildar.Wallet.Bot.DataAccess;

public sealed class BotDbContext : DbContext
{
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

    public DbSet<TelegramUser> Users { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Record> Records { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TelegramUser>()
            .HasMany(x => x.Records)
            .WithOne(x => x.TelegramUser)
            .HasForeignKey(b => b.TelegramUserId);
    }

}