using Domain.Enum;

namespace Domain.Class;

public class Service
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public long BasePrice { get; set; }
    public ServiceEnums.PaymentTerm DefaultPaymentTerm { get; set; }
    public string? Category { get; set; }
        
    public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    
    
}