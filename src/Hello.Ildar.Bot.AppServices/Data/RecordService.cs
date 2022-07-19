using Hello.Ildar.Bot.Contracts;
using Hello.Ildar.Bot.DataAccess;
using Hello.Ildar.Bot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hello.Ildar.Bot.AppServices.Data;

public class RecordService : IRecordService
{
    private readonly BotDbContext _context;

    public RecordService(BotDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddAsync(RecordDto recordDto, CancellationToken ct)
    {
        var userEntity = await _context.Records.AddAsync(
            new Record
            {
                TelegramUserId = recordDto.UserId,
                Value = recordDto.Value,
                CategoryId = recordDto.CategoryId,
            }, ct);

        await _context.SaveChangesAsync(ct);

        return userEntity.Entity.Id;
    }

    public async Task UpdateCategoryAsync(int recordId, int categoryId, CancellationToken ct)
    {
        var record = await _context.Records.FirstOrDefaultAsync(x => x.Id == recordId, cancellationToken: ct);

        if (record == null)
        {
            throw new Exception($"Record with id {recordId} not found.");
        }
        
        record.CategoryId = categoryId;
        await _context.SaveChangesAsync(ct);
    }

    public async Task<decimal> GetBalanceAsync(int userId, CancellationToken ct)
    {
        var result = await _context.Records
            .Where(x => x.TelegramUserId == userId)
            .SumAsync(x => x.Value, cancellationToken: ct);

        return result;
    }

    public async Task<decimal> GetIncomeSumAsync(int userId, CancellationToken ct)
    {
        var result = await _context.Records
            .Where(x => x.TelegramUserId == userId)
            .Where(x => x.Value > 0)
            .SumAsync(x => x.Value, cancellationToken: ct);

        return result;
    }

    public async Task<decimal> GetSpendingSumAsync(int userId, CancellationToken ct)
    {
        var result = await _context.Records
            .Where(x => x.TelegramUserId == userId)
            .Where(x => x.Value < 0)
            .SumAsync(x => x.Value, cancellationToken: ct);

        return result;
    }

    public async Task<decimal> GetIncomeSumCategorizedAsync(int userId, CancellationToken ct)
    {
        var result = await _context.Records
            .Where(x => x.TelegramUserId == userId)
            .Where(x => x.Value > 0)
            .SumAsync(x => x.Value, cancellationToken: ct);

        return result;
    }

    public async Task<decimal> GetSpendingSumCategorizedAsync(int userId, CancellationToken ct)
    {
        var result = await _context.Records
            .Where(x => x.TelegramUserId == userId)
            .Where(x => x.Value < 0)
            .SumAsync(x => x.Value, cancellationToken: ct);

        return result;
    }
}