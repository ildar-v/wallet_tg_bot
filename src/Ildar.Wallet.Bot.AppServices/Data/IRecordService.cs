using Ildar.Wallet.Bot.Common;
using Ildar.Wallet.Bot.Contracts;
using Telegram.Bot.Types;

namespace Ildar.Wallet.Bot.AppServices.Data;

public interface IRecordService
{
    Task<int> AddAsync(RecordDto recordDto, CancellationToken ct);

    Task UpdateCategoryAsync(int recordId, int categoryId, CancellationToken ct);

    Task<decimal> GetBalanceAsync(int userId, CancellationToken ct);

    Task<decimal> GetIncomeSumAsync(int userId, CancellationToken ct);

    Task<decimal> GetSpendingSumAsync(int userId, CancellationToken ct);

    Task<decimal> GetIncomeSumCategorizedAsync(int userId, CancellationToken ct);

    Task<decimal> GetSpendingSumCategorizedAsync(int userId, CancellationToken ct);
}