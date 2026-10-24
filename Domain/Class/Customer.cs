namespace Domain.Class;

public class Customer
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    
    public long Balance { get; set; }
    public long WithdrawBalance { get; set; }
    public long BonusBalance { get; set; }
        
    public bool BonusState { get; set; }
    public long TotalReplenished { get; set; }
}