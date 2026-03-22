using DesafioSupplier.Domain.Entities;

namespace DesafioSupplier.Domain.Interfaces.Repositories;

public interface ICustomerRepository
{
    Task<Customer> GetCustomerAsync(string cpf);
    Task SaveCustomerAsync(Customer customer);
    Task<List<Customer>> GetAllCustomersAsync();
}
