using Hello.Ildar.Bot.DataAccess;
using Hello.Ildar.Bot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hello.Ildar.Bot.AppServices;

public class BotDbService : IBotDbService
{
    private readonly BotDbContext _context;

    public BotDbService(BotDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddCategory(string name)
    {
        var res = await _context.Categories.AddAsync(new Category { Name = name });
        await _context.SaveChangesAsync();
        return res.Entity.Id;
    }

    public async Task<string> GetAllCategories()
    {
        var res = await _context.Categories.OrderByDescending(x => x.Id).Select(x => x.Name).ToArrayAsync();
        return string.Join(", ", res);
    }
}