using Hello.Ildar.Bot.Common;
using Hello.Ildar.Bot.Contracts;

namespace Hello.Ildar.Bot.AppServices.Data;

public interface ICategoryService
{
    Task<int> AddAsync(string name, CancellationToken ct);

    Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken ct, int count = AppConstants.MaxDbEntriesCount);
}