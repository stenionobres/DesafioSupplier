using Moq;
using Xunit;
using DesafioSupplier.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;
using DesafioSupplier.Application.Services;
using DesafioSupplier.Domain.Interfaces.Repositories;

namespace DesafioSupplier.Tests;

public class CustomerServiceTests
{
    private readonly Mock<ICustomerRepository> _repositoryMock;
    private readonly IMemoryCache _memoryCache;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly CustomerService _service;

    public CustomerServiceTests()
    {
        _repositoryMock = new Mock<ICustomerRepository>();
        _memoryCache = new MemoryCache(new MemoryCacheOptions());

        _configurationMock = new Mock<IConfiguration>();

        _configurationMock.Setup(c => c["CacheSettings:AllCustomersKey"])
            .Returns("all_customers");

        _configurationMock.Setup(c => c["CacheSettings:AllCustomersDurationInMinutes"])
            .Returns("10");

        _service = new CustomerService(
            _memoryCache,
            _repositoryMock.Object,
            _configurationMock.Object
        );
    }

    [Fact]
    public async Task SaveCustomerAsync_ShouldSaveCustomer_WhenCpfDoesNotExist()
    {
        var customer = new Customer
        {
            Id = string.Empty,
            Name = "Test",
            Cpf = "12345678900",
            LimitValue = 1000
        };

        _repositoryMock
            .Setup(r => r.GetCustomerByCpfAsync(customer.Cpf))
            .ReturnsAsync((Customer)null);

        _repositoryMock
            .Setup(r => r.SaveCustomerAsync(It.IsAny<Customer>()))
            .Returns(Task.CompletedTask);

        var result = await _service.SaveCustomerAsync(customer);

        Assert.False(string.IsNullOrEmpty(result));
        Assert.Equal(customer.Id, result);

        _repositoryMock.Verify(r =>
            r.SaveCustomerAsync(It.Is<Customer>(c =>
                c.Id == result &&
                c.Cpf == customer.Cpf
            )), Times.Once);
    }

    [Fact]
    public async Task SaveCustomerAsync_ShouldThrowException_WhenCpfAlreadyExists()
    {
        var customer = new Customer 
        { 
            Id = Guid.NewGuid().ToString(),
            Name = "Customer 01",
            Cpf = "12345678900",
            LimitValue = 10
        };

        _repositoryMock
            .Setup(r => r.GetCustomerByCpfAsync(customer.Cpf))
            .ReturnsAsync(customer);

        var ex = await Assert.ThrowsAsync<ApplicationException>(() =>
            _service.SaveCustomerAsync(customer));

        Assert.Equal("Já existe cliente com esse Cpf", ex.Message);

        _repositoryMock.Verify(r =>
            r.SaveCustomerAsync(It.IsAny<Customer>()), Times.Never);
    }

    [Fact]
    public async Task GetAllCustomersAsync_ShouldReturnFromRepository_AndCacheResult()
    {
        var customers = new List<Customer>
        {
            new Customer { Id = "1", Name = "Customer 01", Cpf = "123", LimitValue = 10 }
        };

        _repositoryMock
            .Setup(r => r.GetAllCustomersAsync())
            .ReturnsAsync(customers);

        var result1 = await _service.GetAllCustomersAsync();
        var result2 = await _service.GetAllCustomersAsync();

        Assert.Equal(customers, result1);
        Assert.Equal(customers, result2);

        _repositoryMock.Verify(r =>
            r.GetAllCustomersAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateLimit_ShouldUpdateLimit_WhenValid()
    {
        var customer = new Customer
        {
            Id = "1",
            Name = "Customer 01",
            Cpf = "123",
            LimitValue = 1000
        };

        var debitAmount = 200;

        _repositoryMock
            .Setup(r => r.GetCustomerByIdAsync(customer.Id))
            .ReturnsAsync(customer);

        _repositoryMock
            .Setup(r => r.UpdateLimit(customer.Id, 800))
            .Returns(Task.CompletedTask);

        await _service.UpdateLimit(customer.Id, debitAmount);

        _repositoryMock.Verify(r =>
            r.UpdateLimit(customer.Id, 800), Times.Once);
    }

    [Fact]
    public async Task UpdateLimit_ShouldThrowException_WhenCustomerNotFound()
    {
        _repositoryMock
            .Setup(r => r.GetCustomerByIdAsync("1"))
            .ReturnsAsync((Customer)null);

        var ex = await Assert.ThrowsAsync<ApplicationException>(() =>
            _service.UpdateLimit("1", 100));

        Assert.Equal("Não existe cliente com esse Id", ex.Message);

        _repositoryMock.Verify(r =>
            r.UpdateLimit(It.IsAny<string>(), It.IsAny<decimal>()), Times.Never);
    }

    [Fact]
    public async Task UpdateLimit_ShouldThrowException_WhenDebitExceedsLimit()
    {
        var customer = new Customer
        {
            Id = "1",
            Name = "Customer Teste",
            Cpf = "13565",
            LimitValue = 100
        };

        _repositoryMock
            .Setup(r => r.GetCustomerByIdAsync(customer.Id))
            .ReturnsAsync(customer);

        var ex = await Assert.ThrowsAsync<ApplicationException>(() =>
            _service.UpdateLimit(customer.Id, 200));

        Assert.Equal("Valor solicitado é superior ao limite do cliente", ex.Message);

        _repositoryMock.Verify(r =>
            r.UpdateLimit(It.IsAny<string>(), It.IsAny<decimal>()), Times.Never);
    }
}
