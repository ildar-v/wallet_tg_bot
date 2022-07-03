using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Hello.Ildar.Bot.AppServices.Bot;

public class BotService : IBotService
{
    private readonly IBotDbService _botDbService;

    public BotService(IBotDbService botDbService)
    {
        _botDbService = botDbService;
    }

    public async Task StartBot(CancellationToken ct)
    {

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
        // Only process Message updates: https://core.telegram.org/bots/api#message
        if (update.Type != UpdateType.Message)
            return;

        // Only process text messages
        if (update.Message!.Type != MessageType.Text)
            return;

        var chatId = update.Message.Chat.Id;
        var messageText = update.Message.Text;

        Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
        
        // var message = $"{update.Message.Chat.FirstName}, вы написали :\n"
        //               + messageText
        //               + $"\nВаша ссылка в телегу: https://t.me/{update.Message.Chat.Username}";
        var message = string.Empty;

        var categoryCommand = messageText?.Split(" ");
        if (categoryCommand != null
            && categoryCommand.Length > 1
            && categoryCommand[0].ToLower().Trim() == "добавить")
        {
            var categoryName = string.Join(
                " ",
                categoryCommand.Skip(1));

            categoryName = categoryName.Substring(0, categoryName.Length < 50 ? categoryName.Length : 50);
            var addedCategoryId  = await _botDbService.AddCategory(categoryName);
            message = $"Категория {categoryName} была добавлена с Id {addedCategoryId}";
        }

        if (messageText == "категории")
        {
            var allCategories = await _botDbService.GetAllCategories();
            message = $"Все категории: {allCategories}";
        }

        if (!string.IsNullOrWhiteSpace(message))
        {
            var sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: message,
                cancellationToken: ct);
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