using Hello.Ildar.Bot.Contracts;
using Telegram.Bot.Types;

namespace Hello.Ildar.Bot.AppServices.Bot;

public interface IBotAnswerService
{
    Task<BotAnswerDto?> Get(Message? message, TelegramUserDto user, CancellationToken ct);
}