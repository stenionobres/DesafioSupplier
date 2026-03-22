using DesafioSupplier.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;
using DesafioSupplier.Domain.Interfaces.Services;
using DesafioSupplier.Domain.Interfaces.Repositories;

namespace DesafioSupplier.Application.Services;

public class CustomerService(IMemoryCache memoryCache, ICustomerRepository customerRepository, IConfiguration configuration) : ICustomerService
{
    private string _allCustomersCacheKey => configuration["CacheSettings:AllCustomersKey"]!;
    private int _allCustomersCacheDuration => int.Parse(configuration["CacheSettings:AllCustomersDurationInMinutes"]!);

    public async Task<string> SaveCustomerAsync(Customer customer)
    {
        var savedCustomer = await customerRepository.GetCustomerByCpfAsync(customer.Cpf);

        if (savedCustomer != null)
            throw new ApplicationException("Já existe cliente com esse Cpf");

        customer.Id = Guid.NewGuid().ToString();

        await customerRepository.SaveCustomerAsync(customer);
        
        return customer.Id;
    }

    public async Task<List<Customer>> GetAllCustomersAsync()
    {
        var customers = await memoryCache.GetOrCreateAsync(_allCustomersCacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_allCustomersCacheDuration);
            var customers = await customerRepository.GetAllCustomersAsync();
            
            return customers;
        });
        
        return customers!;
    }

    public async Task UpdateLimit(string customerId, decimal debitAmount)
    {
        var savedCustomer = await customerRepository.GetCustomerByIdAsync(customerId);

        if (savedCustomer == null)
            throw new ApplicationException("Não existe cliente com esse Id");

        if (debitAmount > savedCustomer.LimitValue)
            throw new ApplicationException("Valor solicitado é superior ao limite do cliente");

        var newLimit = savedCustomer.LimitValue - debitAmount;
        
        await customerRepository.UpdateLimit(savedCustomer.Id, newLimit);
    }
}
