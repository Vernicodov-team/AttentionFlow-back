using System.Text.Json.Serialization;

namespace Application.DTO;

/// <summary>
/// Снапшот исходных данных: три листа — customers, service, transactions.
/// </summary>
public class SnapshotDto
{
    [JsonPropertyName("customers")]
    public List<SnapshotCustomerDto>? Customers { get; set; }

    [JsonPropertyName("service")]
    public List<SnapshotServiceDto>? Service { get; set; }

    /// <summary>Допустимый синоним листа service (множественное число).</summary>
    [JsonPropertyName("services")]
    public List<SnapshotServiceDto>? Services { get; set; }

    [JsonPropertyName("transactions")]
    public List<SnapshotTransactionDto>? Transactions { get; set; }
}

public class SnapshotCustomerDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("balance")]
    public decimal? Balance { get; set; }

    [JsonPropertyName("withdrawBalance")]
    public decimal? WithdrawBalance { get; set; }

    [JsonPropertyName("bonusBalance")]
    public decimal? BonusBalance { get; set; }

    [JsonPropertyName("bonusState")]
    public bool? BonusState { get; set; }

    [JsonPropertyName("totalReplenished")]
    public decimal? TotalReplenished { get; set; }
}

public class SnapshotServiceDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>Владелец услуги (в презентации — accountId).</summary>
    [JsonPropertyName("customerId")]
    public string? CustomerId { get; set; }

    [JsonPropertyName("accountId")]
    public string? AccountId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("price")]
    public decimal? Price { get; set; }

    [JsonPropertyName("autoprolong")]
    public bool? Autoprolong { get; set; }

    [JsonPropertyName("paymentTerm")]
    public string? PaymentTerm { get; set; }

    [JsonPropertyName("expiresAt")]
    public string? ExpiresAt { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }
}

public class SnapshotTransactionDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>Владелец транзакции (в презентации — accountId).</summary>
    [JsonPropertyName("customerId")]
    public string? CustomerId { get; set; }

    [JsonPropertyName("accountId")]
    public string? AccountId { get; set; }

    [JsonPropertyName("serviceId")]
    public string? ServiceId { get; set; }

    [JsonPropertyName("amount")]
    public decimal? Amount { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("createdAt")]
    public string? CreatedAt { get; set; }
}
