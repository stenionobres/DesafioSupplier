using DesafioSupplier.Domain.Entities;

namespace DesafioSupplier.Domain.Interfaces.Services;

public interface ICustomerService
{
    Task<string> SaveCustomerAsync(Customer customer);
    Task<List<Customer>> GetAllCustomersAsync();
}
