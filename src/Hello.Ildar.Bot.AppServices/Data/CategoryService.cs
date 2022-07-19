using Hello.Ildar.Bot.Common;
using Hello.Ildar.Bot.Contracts;
using Hello.Ildar.Bot.DataAccess;
using Hello.Ildar.Bot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hello.Ildar.Bot.AppServices.Data;

public class CategoryService : ICategoryService
{
    private readonly BotDbContext _context;

    public CategoryService(BotDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddAsync(string name, CancellationToken ct)
    {
        var alreadyCreatedCategory =
            await _context.Categories.FirstOrDefaultAsync(x => x.Name.ToLower().Trim() == name.ToLower().Trim(),
                cancellationToken: ct);

        if (alreadyCreatedCategory != null)
        {
            return alreadyCreatedCategory.Id;
        }

        var res = await _context.Categories.AddAsync(new Category { Name = name }, ct);

        await _context.SaveChangesAsync(ct);

        return res.Entity.Id;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken ct, int count = AppConstants.MaxDbEntriesCount)
    {
        count = count > AppConstants.MaxDbEntriesCount ? AppConstants.MaxDbEntriesCount : count;

        var res = await _context.Categories
            .OrderByDescending(x => x.Id)
            .Take(count)
            .Select(x => new CategoryDto { Id = x.Id, Name = x.Name })
            .ToArrayAsync(cancellationToken: ct);

        return res;
    }
}