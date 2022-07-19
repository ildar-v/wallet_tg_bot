using Hello.Ildar.Bot.Contracts;
using Hello.Ildar.Bot.DataAccess;
using Hello.Ildar.Bot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace Hello.Ildar.Bot.AppServices.Data;

public class TelegramUserService : ITelegramUserService
{
    private readonly BotDbContext _context;

    public TelegramUserService(BotDbContext context)
    {
        _context = context;
    }

    public async Task<TelegramUserDto> AddAsync(Chat chat, CancellationToken ct)
    {
        var alreadyCreatedUser =
            await _context.Users.FirstOrDefaultAsync(x => x.ChatId == chat.Id, cancellationToken: ct);

        if (alreadyCreatedUser != null)
        {
            return GetDto(alreadyCreatedUser);
        }

        var entry = await _context.Users.AddAsync(
            new TelegramUser
            {
                Title = chat.Title,
                Username = chat.Username,
                ChatId = chat.Id,
                ChatType = (int)chat.Type,
                FirstName = chat.FirstName,
                LastName = chat.LastName,
                SerializedDataJson = JsonConvert.SerializeObject(chat),
            }, ct);

        await _context.SaveChangesAsync(ct);

        return GetDto(entry.Entity);
    }

    public async Task UpdateLastRecordAsync(int userId, int lastRecordId, CancellationToken ct)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken: ct);

        if (user == null)
        {
            throw new Exception($"TelegramUser with id {userId} not found.");
        }

        user.LastRecordId = lastRecordId;
        await _context.SaveChangesAsync(ct);
    }

    private TelegramUserDto GetDto(TelegramUser userEntity)
    {
        return new TelegramUserDto
        {
            Id = userEntity.Id,
            Title = userEntity.Title,
            Username = userEntity.Username,
            ChatId = userEntity.ChatId,
            ChatType = userEntity.ChatType,
            FirstName = userEntity.FirstName,
            LastName = userEntity.LastName,
            IsAdmin = userEntity.IsAdmin,
            LastRecordId = userEntity.LastRecordId,
        };
    }
}