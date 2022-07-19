using Ildar.Wallet.Bot.Contracts;
using Telegram.Bot.Types;

namespace Ildar.Wallet.Bot.AppServices.Bot;

public interface IBotAnswerService
{
    Task<BotAnswerDto?> Get(Message? message, TelegramUserDto user, CancellationToken ct);
}