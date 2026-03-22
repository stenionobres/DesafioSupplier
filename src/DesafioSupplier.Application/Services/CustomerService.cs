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
            throw new ApplicationException("Cliente já existe com esse Cpf");

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
}
