namespace Hello.Ildar.Bot.Domain.Entities;

/// <summary>
/// Запись о расходах/доходах.
/// </summary>
public class Record : EntityBase
{
    public User User;
    
    public Category Category;
    
    public decimal Value { get; set; }
}