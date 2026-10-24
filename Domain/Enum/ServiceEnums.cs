namespace Domain.Enum;

public class ServiceEnums
{
    public enum PaymentTerm
    {
        Hour,
        Week,
        Month,
        QuarterYear,
        HalfYear,
        Year
    }

    public enum ServiceStatus
    {
        Active,
        Suspended,
        ProlongWait,
        Deleted,
        Blocked
    }

    public enum TransactionType
    {
        Prolong,
        Order
    }

    public enum TransactionStatus
    {
        Performed,
        NotPerformed
    }
}