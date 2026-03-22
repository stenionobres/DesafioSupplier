using DesafioSupplier.Domain.Entities;

namespace DesafioSupplier.Domain.Interfaces.Repositories;

public interface ICustomerRepository
{
    Task<Customer> GetCustomerByCpfAsync(string cpf);
    Task<Customer> GetCustomerByIdAsync(string customerId);
    Task SaveCustomerAsync(Customer customer);
    Task<List<Customer>> GetAllCustomersAsync();
}
