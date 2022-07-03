namespace Hello.Ildar.Bot.AppServices;

public interface IBotDbService
{
    Task<int> AddCategory(string name);

    Task<string> GetAllCategories();
}