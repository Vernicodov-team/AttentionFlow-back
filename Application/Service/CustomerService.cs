using DB.Repositories.Interfaces;
using Domain.Class;

namespace Application.Service;

public class CustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    
    
    
}