using Ildar.Wallet.Bot.Contracts;
using Telegram.Bot.Types;

namespace Ildar.Wallet.Bot.AppServices.Data;

public interface ITelegramUserService
{
    Task<TelegramUserDto> AddAsync(Chat chat, CancellationToken ct);

    Task UpdateLastRecordAsync(int userId, int lastRecordId, CancellationToken ct);
}