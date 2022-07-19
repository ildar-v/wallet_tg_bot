using Ildar.Wallet.Bot.AppServices.Data;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Ildar.Wallet.Bot.AppServices.Bot;

public class BotService : IBotService
{
    private readonly ITelegramUserService _telegramUserService;
    private readonly IBotAnswerService _botAnswerService;
    private readonly IConfiguration _configuration;

    public BotService(
        ITelegramUserService telegramUserService,
        IBotAnswerService botAnswerService,
        IConfiguration configuration)
    {
        _telegramUserService = telegramUserService;
        _botAnswerService = botAnswerService;
        _configuration = configuration;
    }

    public async Task StartBot(CancellationToken ct)
    {
        var token = _configuration.GetSection("TelegramBot:Token").Value
                    ?? throw new ArgumentNullException("Configuration hasn't value for TelegramBot:Token");

        var botClient = new TelegramBotClient(token);

        // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
        };

        botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: ct
        );

        var me = await botClient.GetMeAsync(ct);

        Console.WriteLine($"Start listening for @{me.Username}");
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
    {
        try
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Type != UpdateType.Message)
                return;

            // Only process text messages
            if (update.Message!.Type != MessageType.Text)
                return;

            var chatId = update.Message.Chat.Id;

            Console.WriteLine($"Received a '{update.Message.Text}' message in chat {chatId}.");

            var currentUser = await _telegramUserService.AddAsync(update.Message.Chat, ct);

            var answer = await _botAnswerService.Get(update.Message, currentUser, ct);
            // update.Message.MessageId

            if (answer != null)
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: answer.Text,
                    cancellationToken: ct,
                    replyMarkup: answer.ReplyMarkup,
                    replyToMessageId: update.Message.MessageId,
                    allowSendingWithoutReply: false
                );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        catch
        {
            Console.WriteLine("Произошла непредвиденная ошибка!");
        }
    }

    private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }

    // private void ReleaseUnmanagedResources()
    // {
    //     // TODO release unmanaged resources here
    // }
    //
    // public void Dispose()
    // {
    //     ReleaseUnmanagedResources();
    //     GC.SuppressFinalize(this);
    // }
    //
    // ~BotService()
    // {
    //     ReleaseUnmanagedResources();
    // }
}