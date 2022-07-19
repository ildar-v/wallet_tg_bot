namespace Ildar.Wallet.Bot.AppServices.Bot;

public interface IBotService //: IDisposable
{
    Task StartBot(CancellationToken ct);
}