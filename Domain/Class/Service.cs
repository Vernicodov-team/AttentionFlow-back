using Domain.Enum;

namespace Domain.Class;

public class Service
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public string Name { get; set; } = string.Empty;

    public long Price { get; set; }

    public bool Autoprolong { get; set; }

    public ServiceEnums.PaymentTerm PaymentTerm { get; set; }

    public ServiceEnums.ServiceStatus Status { get; set; }

    public DateTime ExpiresAt { get; set; }

    public virtual Customer? Customer { get; set; }
}
