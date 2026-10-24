namespace Application.DTO;

public class CustomerDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public double Balance { get; set; }
    public double SumReceived { get; set; }
    public double RiskScore { get; set; }
    public double RiskSumm { get; set; }
}