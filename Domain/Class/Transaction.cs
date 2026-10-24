using Domain.Enum;

namespace Domain.Class;

public class Transaction
{
    public int Id { get; set; }

    public Guid CustomerId { get; set; }

    public int? ServiceId { get; set; }

    public long Amount { get; set; }

    public ServiceEnums.TransactionType Type { get; set; }

    public ServiceEnums.TransactionStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Service? Service { get; set; }
}
