namespace Hello.Ildar.Bot.AppServices.Bot;

public interface IBotService //: IDisposable
{
    Task StartBot(CancellationToken ct);
}