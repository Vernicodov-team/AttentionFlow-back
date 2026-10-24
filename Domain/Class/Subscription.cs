using Domain.Enum;

namespace Domain.Class;

public class Subscription
{
    public int Id { get; set; }
    public Guid CustomerId { get; set; }
    public int ServiceId { get; set; }
    public string? CustomName { get; set; }
    public long ActualPrice { get; set; }
    public bool Autoprolong { get; set; }
    public ServiceEnums.PaymentTerm PaymentTerm { get; set; }
    public ServiceEnums.ServiceStatus Status { get; set; }
    public DateTime ExpiresAt { get; set; }
        
    public virtual Customer? Customer { get; set; }
    public virtual Service? Service { get; set; }
}