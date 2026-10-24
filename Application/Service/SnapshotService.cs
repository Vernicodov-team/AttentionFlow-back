using System.Globalization;
using System.Text.Json;
using Application.DTO;
using DB.Repositories.Interfaces;
using Domain.Class;
using Domain.Enum;
using ServiceEntity = Domain.Class.Service;

namespace Application.Service;

public class SnapshotService : ISnapshotService
{
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        AllowTrailingCommas = true,
        ReadCommentHandling = JsonCommentHandling.Skip
    };

    private readonly ISnapshotRepository _snapshotRepository;

    public SnapshotService(ISnapshotRepository snapshotRepository)
    {
        _snapshotRepository = snapshotRepository;
    }

    public async Task<SnapshotImportResultDto> ReplaceFromJsonAsync(
        Stream json,
        CancellationToken cancellationToken = default)
    {
        if (json is null) throw new Exception("SnapshotJsonIsMissing");

        SnapshotDto? snapshot;
        try
        {
            snapshot = await JsonSerializer.DeserializeAsync<SnapshotDto>(json, JsonOptions, cancellationToken);
        }
        catch (JsonException ex)
        {
            throw new Exception($"SnapshotJsonIsInvalid: {ex.Message}");
        }

        if (snapshot is null) throw new Exception("SnapshotJsonIsEmpty");

        return await ReplaceAsync(snapshot, cancellationToken);
    }

    public async Task<SnapshotImportResultDto> ReplaceAsync(
        SnapshotDto snapshot,
        CancellationToken cancellationToken = default)
    {
        if (snapshot is null) throw new Exception("SnapshotIsMissing");

        // Лист service может прийти под именем services — принимаем оба, но не оба сразу.
        if (snapshot.Service is not null && snapshot.Services is not null)
            throw new Exception("SnapshotServiceSheetIsDuplicated");

        List<SnapshotServiceDto> serviceSheet = snapshot.Service ?? snapshot.Services ?? new List<SnapshotServiceDto>();
        List<SnapshotCustomerDto> customerSheet = snapshot.Customers ?? new List<SnapshotCustomerDto>();
        List<SnapshotTransactionDto> transactionSheet = snapshot.Transactions ?? new List<SnapshotTransactionDto>();

        List<Customer> customers = MapCustomers(customerSheet);
        List<ServiceEntity> services = MapServices(serviceSheet, customers);
        List<Transaction> transactions = MapTransactions(transactionSheet, customers, services);

        // Сначала полная проверка, затем замена: при любой ошибке база остаётся нетронутой.
        await _snapshotRepository.ReplaceAllAsync(customers, services, transactions, cancellationToken);

        return new SnapshotImportResultDto
        {
            Customers = customers.Count,
            Services = services.Count,
            Transactions = transactions.Count
        };
    }

    private static List<Customer> MapCustomers(List<SnapshotCustomerDto> rows)
    {
        var customers = new List<Customer>(rows.Count);
        var ids = new HashSet<int>();

        for (int i = 0; i < rows.Count; i++)
        {
            SnapshotCustomerDto row = rows[i];
            string where = $"customers[{i}]";

            int id = ParseId(row.Id, where, "id");
            if (!ids.Add(id)) throw new Exception($"{where}: duplicate id {id}");

            string email = (row.Email ?? string.Empty).Trim();
            if (email.Length == 0) throw new Exception($"{where}: email is required");

            customers.Add(new Customer
            {
                Id = id,
                Email = email,
                Balance = ParseMoney(row.Balance, where, "balance"),
                WithdrawBalance = ParseMoney(row.WithdrawBalance, where, "withdrawBalance"),
                BonusBalance = ParseMoney(row.BonusBalance, where, "bonusBalance"),
                BonusState = row.BonusState ?? false,
                TotalReplenished = ParseMoney(row.TotalReplenished, where, "totalReplenished")
            });
        }

        return customers;
    }

    private static List<ServiceEntity> MapServices(List<SnapshotServiceDto> rows, List<Customer> customers)
    {
        var services = new List<ServiceEntity>(rows.Count);
        var ids = new HashSet<int>();
        var customerIds = customers.Select(c => c.Id).ToHashSet();

        for (int i = 0; i < rows.Count; i++)
        {
            SnapshotServiceDto row = rows[i];
            string where = $"service[{i}]";

            int id = ParseId(row.Id, where, "id");
            if (!ids.Add(id)) throw new Exception($"{where}: duplicate id {id}");

            int customerId = ParseId(row.CustomerId ?? row.AccountId, where, "customerId");
            if (!customerIds.Contains(customerId))
                throw new Exception($"{where}: customer {customerId} is not present in customers sheet");

            string name = (row.Name ?? string.Empty).Trim();
            if (name.Length == 0) throw new Exception($"{where}: name is required");

            services.Add(new ServiceEntity
            {
                Id = id,
                CustomerId = customerId,
                Name = name,
                Price = ParseMoney(row.Price, where, "price"),
                Autoprolong = row.Autoprolong ?? false,
                PaymentTerm = ParseEnum<ServiceEnums.PaymentTerm>(row.PaymentTerm, where, "paymentTerm"),
                Status = ParseEnum<ServiceEnums.ServiceStatus>(row.Status, where, "status"),
                ExpiresAt = ParseDate(row.ExpiresAt, where, "expiresAt")
            });
        }

        return services;
    }

    private static List<Transaction> MapTransactions(
        List<SnapshotTransactionDto> rows,
        List<Customer> customers,
        List<ServiceEntity> services)
    {
        var transactions = new List<Transaction>(rows.Count);
        var ids = new HashSet<int>();
        var customerIds = customers.Select(c => c.Id).ToHashSet();
        var serviceIds = services.Select(s => s.Id).ToHashSet();

        for (int i = 0; i < rows.Count; i++)
        {
            SnapshotTransactionDto row = rows[i];
            string where = $"transactions[{i}]";

            int id = ParseId(row.Id, where, "id");
            if (!ids.Add(id)) throw new Exception($"{where}: duplicate id {id}");

            int customerId = ParseId(row.CustomerId ?? row.AccountId, where, "customerId");
            if (!customerIds.Contains(customerId))
                throw new Exception($"{where}: customer {customerId} is not present in customers sheet");

            int? serviceId = null;
            if (!string.IsNullOrWhiteSpace(row.ServiceId))
            {
                serviceId = ParseId(row.ServiceId, where, "serviceId");
                if (!serviceIds.Contains(serviceId.Value))
                    throw new Exception($"{where}: service {serviceId} is not present in service sheet");
            }

            transactions.Add(new Transaction
            {
                Id = id,
                CustomerId = customerId,
                ServiceId = serviceId,
                Amount = ParseMoney(row.Amount, where, "amount"),
                Type = ParseEnum<ServiceEnums.TransactionType>(row.Type, where, "type"),
                Status = ParseEnum<ServiceEnums.TransactionStatus>(row.Status, where, "status"),
                // В презентации даты транзакции нет — берём момент загрузки.
                CreatedAt = string.IsNullOrWhiteSpace(row.CreatedAt)
                    ? DateTime.UtcNow
                    : ParseDate(row.CreatedAt, where, "createdAt")
            });
        }

        return transactions;
    }

    private static int ParseId(string? value, string where, string field)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new Exception($"{where}: {field} is required");

        if (!int.TryParse(value.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int id) || id <= 0)
            throw new Exception($"{where}: {field} '{value}' is not a positive integer");

        return id;
    }

    private static long ParseMoney(decimal? value, string where, string field)
    {
        if (value is null) throw new Exception($"{where}: {field} is required");
        if (value.Value != decimal.Truncate(value.Value))
            throw new Exception($"{where}: {field} '{value}' must be a whole number");

        if (value.Value < long.MinValue || value.Value > long.MaxValue)
            throw new Exception($"{where}: {field} '{value}' is out of range");

        return (long)value.Value;
    }

    private static TEnum ParseEnum<TEnum>(string? value, string where, string field)
        where TEnum : struct, Enum
    {
        string normalized = (value ?? string.Empty)
            .Replace("_", string.Empty)
            .Replace("-", string.Empty)
            .Replace(" ", string.Empty);

        if (Enum.TryParse(normalized, ignoreCase: true, out TEnum parsed) && Enum.IsDefined(parsed))
            return parsed;

        throw new Exception($"{where}: {field} '{value}' is unknown");
    }

    private static DateTime ParseDate(string? value, string where, string field)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new Exception($"{where}: {field} is required");

        if (!DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out DateTime parsed))
            throw new Exception($"{where}: {field} '{value}' is not a valid date");

        return DateTime.SpecifyKind(parsed, DateTimeKind.Utc);
    }
}
