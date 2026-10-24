using Application.DTO;
using DB.Repositories.Interfaces;

namespace Application.Service;

public class AnalysisService
{
    private readonly ICustomerRepository _customerRepository;

    public AnalysisService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<List<CustomerDto>> CalcAllData()
    {
        var customers = await _customerRepository.GetAllAsync();
        
        var result = new List<CustomerDto>();
        foreach (var customer in customers)
        {
            var dto = new CustomerDto();
            dto.Balance = customer.Balance;
            dto.SumReceived = customer.TotalReplenished;
            dto.Email = customer.Email;
            
            result.Add(dto);
        }
        return result;
    }
}