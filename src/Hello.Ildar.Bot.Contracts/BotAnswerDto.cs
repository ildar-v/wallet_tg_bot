using Telegram.Bot.Types.ReplyMarkups;

namespace Hello.Ildar.Bot.Contracts;

public class BotAnswerDto
{
    public string Text { get; set; }
    public IReplyMarkup? ReplyMarkup { get; set; }
    
    public bool IsReply { get; set; }
}