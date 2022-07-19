using Ildar.Wallet.Bot.Contracts;
using Ildar.Wallet.Bot.AppServices.Data;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Ildar.Wallet.Bot.AppServices.Bot;

public class BotAnswerService : IBotAnswerService
{
    private const string IncomeText = "Income";
    private const string IncomeInputText = "Enter the amount of income";
    private const string SpendingText = "Spending";
    private const string SpendingInputText = "Enter the amount of spending";
    private const string AddCategoryText = "Add category";
    private const string AddCategoryInputText = "Enter name of category";
    private const string CategoriesText = "Categories";
    private const string BalanceText = "Balance";
    private const string IncomeSumText = "Amount of income";
    private const string SpendingSumText = "Amount of expenses";
    private const string IncomeSumCategorizedText = "Amount of income categorized";
    private const string SpendingSumCategorizedText = "Amount of expenses categorized";

    private readonly ICategoryService _categoryService;
    private readonly IRecordService _recordService;
    private readonly ITelegramUserService _telegramUserService;

    public BotAnswerService(ICategoryService categoryService,
        IRecordService recordService,
        ITelegramUserService telegramUserService)
    {
        _categoryService = categoryService;
        _recordService = recordService;
        _telegramUserService = telegramUserService;
    }

    public async Task<BotAnswerDto?> Get(Message? message, TelegramUserDto user, CancellationToken ct)
    {
        var messageText = message?.Text ?? string.Empty;

        if (string.IsNullOrWhiteSpace(messageText))
        {
            return GetMainMenuAnswer(user);
        }

        var allCategories = await _categoryService.GetAllAsync(ct);

        var categoryDtos = allCategories as CategoryDto[] ?? allCategories.ToArray();
        if (user.LastRecordId != null)
        {
            var chosenCategory = categoryDtos.FirstOrDefault(x => x.Name == messageText);

            if (chosenCategory != null)
            {
                await _recordService.UpdateCategoryAsync(user.LastRecordId.Value, chosenCategory.Id, ct);
                return GetMainMenuAnswer(user, "Success. Choose command");
            }
        }

        string answerText;

        switch (messageText)
        {
            case SpendingText:
                return new BotAnswerDto
                {
                    Text = SpendingInputText,
                    ReplyMarkup = new ForceReplyMarkup()
                };
            case IncomeText:
                return new BotAnswerDto
                {
                    Text = IncomeInputText,
                    ReplyMarkup = new ForceReplyMarkup()
                };
            case AddCategoryText:
                return new BotAnswerDto
                {
                    Text = AddCategoryInputText,
                    ReplyMarkup = new ForceReplyMarkup()
                };
            case CategoriesText:
            {
                var catsString = string.Join(", ", categoryDtos.Select(x => x.Name));
                answerText = $"Category list: {catsString}";
                return GetMainMenuAnswer(user, answerText);
            }
            case BalanceText:
                answerText = "Balance: " + await _recordService.GetBalanceAsync(user.Id, ct);
                return GetMainMenuAnswer(user, answerText);

            case IncomeSumText:
                answerText = "Income sum: " + await _recordService.GetIncomeSumAsync(user.Id, ct);
                return GetMainMenuAnswer(user, answerText);

            case SpendingSumText:
                answerText = "Spending sum: " + await _recordService.GetSpendingSumAsync(user.Id, ct);
                return GetMainMenuAnswer(user, answerText);

            case IncomeSumCategorizedText:
                answerText = "Income sum categorized: " +
                             await _recordService.GetIncomeSumCategorizedAsync(user.Id, ct);
                return GetMainMenuAnswer(user, answerText);

            case SpendingSumCategorizedText:
                answerText = "Spending sum categorized: " +
                             await _recordService.GetSpendingSumCategorizedAsync(user.Id, ct);
                return GetMainMenuAnswer(user, answerText);
        }

        var replyMessageText = message?.ReplyToMessage?.Text ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(replyMessageText))
        {
            switch (replyMessageText)
            {
                case IncomeInputText:
                {
                    return await AddRecord(messageText, user.Id, true, ct);
                }
                case SpendingInputText:
                {
                    return await AddRecord(messageText, user.Id, false, ct);
                }
                case AddCategoryInputText:
                {
                    var categoryName = messageText.Substring(0, messageText.Length < 50 ? messageText.Length : 50);

                    var addedCategoryId = await _categoryService.AddAsync(categoryName, ct);

                    return GetMainMenuAnswer(user, $"Category \"{categoryName}\" has been added. Id = {addedCategoryId}");
                }
            }
        }

        return GetMainMenuAnswer(user);
    }

    private async Task<BotAnswerDto> AddRecord(string messageText, int userId, bool isIncome, CancellationToken ct)
    {
        if (!decimal.TryParse(messageText, out var value))
        {
            return new BotAnswerDto()
            {
                Text = $"Unable to parse a number from text \"{messageText}\""
            };
        }

        if (value == 0)
        {
            return new BotAnswerDto()
            {
                Text = "Number can not be zero."
            };
        }

        if (isIncome)
        {
            value = value > 0 ? value : value * -1;
        }
        else
        {
            value = value < 0 ? value : value * -1;
        }

        var recordId = await _recordService.AddAsync(new RecordDto
        {
            Value = value,
            CategoryId = 1,
            UserId = userId
        }, ct);

        await _telegramUserService.UpdateLastRecordAsync(userId, recordId, ct);

        return await GetChooseCategoryAnswer(ct);
    }

    private BotAnswerDto GetMainMenuAnswer(TelegramUserDto user, string? text = null)
    {
        var keyboardButtons = new List<List<KeyboardButton>>
        {
            new List<KeyboardButton>
            {
                new KeyboardButton(IncomeText),
                new KeyboardButton(SpendingText),
                new KeyboardButton(BalanceText),
            },
            new List<KeyboardButton>
            {
                new KeyboardButton(IncomeSumText),
                new KeyboardButton(SpendingSumText),
            },
            new List<KeyboardButton>
            {
                new KeyboardButton(IncomeSumCategorizedText),
                new KeyboardButton(SpendingSumCategorizedText),
            }
        };

        if (user.IsAdmin)
        {
            keyboardButtons.Add(
                new List<KeyboardButton>
                {
                    new KeyboardButton(AddCategoryText),
                    new KeyboardButton(CategoriesText)
                });
        }

        var rkm = new ReplyKeyboardMarkup(keyboardButtons);
        // var rkm = new InlineKeyboardMarkup(new InlineKeyboardButton("cats") { Url = "https://google.com/" });

        return new BotAnswerDto()
        {
            Text = string.IsNullOrWhiteSpace(text) ? "Choose command" : text,
            ReplyMarkup = rkm
        };
    }

    private async Task<BotAnswerDto> GetChooseCategoryAnswer(CancellationToken ct)
    {
        const int MaxCategoriesInRow = 6;
        var allCategories = await _categoryService.GetAllAsync(ct);

        var categories = allCategories.Select(x => x.Name).ToArray();
        var rowsCount = categories.Length / MaxCategoriesInRow;

        var keyboardButtons = new List<List<KeyboardButton>>();

        for (var i = 0; i <= rowsCount; i++)
        {
            var categoryNames = categories.Skip(i * MaxCategoriesInRow).Take(MaxCategoriesInRow);
            keyboardButtons.Add(new List<KeyboardButton>(categoryNames.Select(x => new KeyboardButton(x))));
        }

        var rkm = new ReplyKeyboardMarkup(keyboardButtons);

        return new BotAnswerDto()
        {
            Text = "Choose category",
            ReplyMarkup = rkm,
            IsReply = true
        };
    }
}