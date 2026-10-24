using Domain.Class;

namespace DB.Repositories.Interfaces;

public interface ISnapshotRepository
{
    /// <summary>
    /// Полностью заменяет содержимое таблиц Customers, Services и Transactions
    /// в одной транзакции базы данных: прежние строки удаляются, новые вставляются
    /// с сохранением исходных идентификаторов.
    /// </summary>
    Task ReplaceAllAsync(
        IReadOnlyList<Customer> customers,
        IReadOnlyList<Service> services,
        IReadOnlyList<Transaction> transactions,
        CancellationToken cancellationToken = default);
}
