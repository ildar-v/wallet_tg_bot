using Ildar.Wallet.Bot.Common;
using Ildar.Wallet.Bot.Contracts;

namespace Ildar.Wallet.Bot.AppServices.Data;

public interface ICategoryService
{
    Task<int> AddAsync(string name, CancellationToken ct);

    Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken ct, int count = AppConstants.MaxDbEntriesCount);
}