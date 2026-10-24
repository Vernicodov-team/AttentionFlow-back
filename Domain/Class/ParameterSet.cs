namespace Domain.Class;

public class ParameterSet
{
    public int Id { get; set; }
    public required string PSetName { get; set; }
    
    public double WeightNoAutoprolong { get; set; }
    public double WeightLowBalance { get; set; }
    public double WeightExpiringSoon { get; set; }
    public double WeightProlongWaitStatus { get; set; }
    
    public double Risk { get; set; }
    
    public int FinancialLossHorizonMonths { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}