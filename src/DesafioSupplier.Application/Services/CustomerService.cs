using DesafioSupplier.Domain.Entities;
using DesafioSupplier.Domain.Interfaces.Services;

namespace DesafioSupplier.Application.Services;

public class CustomerService : ICustomerService
{
    public Task<string> SaveCustomerAsync(Customer customer)
    {
        return Task.FromResult("IdCustomer");
    }
}
