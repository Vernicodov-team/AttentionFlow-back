using DB.Repositories.Interfaces;
using Domain.Class;
using Microsoft.EntityFrameworkCore;

namespace DB.Repositories;

public class SnapshotRepository : ISnapshotRepository
{
    private readonly AppDbContext _context;

    public SnapshotRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task ReplaceAllAsync(
        IReadOnlyList<Customer> customers,
        IReadOnlyList<Service> services,
        IReadOnlyList<Transaction> transactions,
        CancellationToken cancellationToken = default)
    {
        await using var dbTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        // Порядок важен: сначала зависимые таблицы, затем родительская.
        await _context.Transactions.ExecuteDeleteAsync(cancellationToken);
        await _context.Services.ExecuteDeleteAsync(cancellationToken);
        await _context.Customers.ExecuteDeleteAsync(cancellationToken);

        // Вставка по уровням связи: клиенты → услуги → транзакции.
        if (customers.Count > 0)
        {
            await _context.Customers.AddRangeAsync(customers, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        if (services.Count > 0)
        {
            await _context.Services.AddRangeAsync(services, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        if (transactions.Count > 0)
        {
            await _context.Transactions.AddRangeAsync(transactions, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        // Идентификаторы вставлены явно, поэтому счётчики identity нужно подвинуть,
        // иначе следующая вставка без Id упрётся в занятое значение.
        await ResetSequenceAsync("Customers", cancellationToken);
        await ResetSequenceAsync("Services", cancellationToken);
        await ResetSequenceAsync("Transactions", cancellationToken);

        await dbTransaction.CommitAsync(cancellationToken);
    }

    private async Task ResetSequenceAsync(string table, CancellationToken cancellationToken)
    {
        // pg_get_serial_sequence принимает имя таблицы как в SQL (с кавычками для PascalCase),
        // а имя столбца — простым текстом, без кавычек.
        string sql =
            $"SELECT setval(pg_get_serial_sequence('\"{table}\"', 'Id'), " +
            $"COALESCE((SELECT MAX(\"Id\") FROM \"{table}\"), 1), " +
            $"(SELECT COUNT(*) FROM \"{table}\") > 0);";

        await _context.Database.ExecuteSqlRawAsync(sql, cancellationToken);
    }
}
