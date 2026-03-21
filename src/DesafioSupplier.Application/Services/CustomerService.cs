using DesafioSupplier.Domain.Entities;
using DesafioSupplier.Domain.Interfaces.Services;
using DesafioSupplier.Domain.Interfaces.Repositories;

namespace DesafioSupplier.Application.Services;

public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
{
    public async Task<string> SaveCustomerAsync(Customer customer)
    {
        customer.Id = Guid.NewGuid().ToString();

        await customerRepository.SaveCustomerAsync(customer);
        
        return customer.Id;
    }

    public async Task<List<Customer>> GetAllCustomersAsync()
    {
        var customers = await customerRepository.GetAllCustomersAsync();
        
        return customers;
    }
}
