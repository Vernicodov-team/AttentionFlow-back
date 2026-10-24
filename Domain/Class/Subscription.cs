using Domain.Enum;

namespace Domain.Class;

public class Subscription
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid CatalogId { get; set; }
    public string? CustomName { get; set; }
    public long ActualPrice { get; set; }
    public bool Autoprolong { get; set; }
    public ServiceEnums.PaymentTerm PaymentTerm { get; set; }
    public ServiceEnums.ServiceStatus Status { get; set; }
    public DateTime ExpiresAt { get; set; }
        
    public virtual Customer? Customer { get; set; }
    public virtual Service? Catalog { get; set; }
}