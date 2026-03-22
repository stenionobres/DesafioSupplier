using Moq;
using Xunit;
using DesafioSupplier.Domain.Entities;
using DesafioSupplier.Application.Services;
using DesafioSupplier.Domain.Interfaces.Publisher;
using DesafioSupplier.Domain.Interfaces.Repositories;

namespace DesafioSupplier.Tests;

public class TransactionServiceTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
    private readonly Mock<IPublisher> _transactionPublisherMock;
    private readonly TransactionService _service;

    public TransactionServiceTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _transactionRepositoryMock = new Mock<ITransactionRepository>();
        _transactionPublisherMock = new Mock<IPublisher>();

        _service = new TransactionService(
            _customerRepositoryMock.Object,
            _transactionRepositoryMock.Object,
            _transactionPublisherMock.Object
        );
    }

    [Fact]
    public async Task PerformTransactionAsync_ShouldSaveAndPublish_WhenValid()
    {
        var customerId = Guid.NewGuid().ToString();
        var transaction = new Transaction
        {
            Id = string.Empty,
            CustomerId = customerId,
            Amount = 100
        };

        var customer = new Customer
        {
            Id = customerId,
            Name = "Customer 01",
            Cpf = "123",
            LimitValue = 500
        };

        _customerRepositoryMock
            .Setup(r => r.GetCustomerByIdAsync(transaction.CustomerId))
            .ReturnsAsync(customer);

        _transactionRepositoryMock
            .Setup(r => r.SaveTransactionAsync(It.IsAny<Transaction>()))
            .Returns(Task.CompletedTask);

        _transactionPublisherMock
            .Setup(p => p.PublishAsync(It.IsAny<Transaction>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.PerformTransactionAsync(transaction);

        // Assert
        Assert.False(string.IsNullOrEmpty(result));
        Assert.Equal(transaction.Id, result);

        _transactionRepositoryMock.Verify(r =>
            r.SaveTransactionAsync(It.Is<Transaction>(t =>
                t.Id == result &&
                t.CustomerId == transaction.CustomerId &&
                t.Amount == transaction.Amount
            )), Times.Once);

        _transactionPublisherMock.Verify(p =>
            p.PublishAsync(It.Is<Transaction>(t =>
                t.Id == result
            )), Times.Once);
    }

    [Fact]
    public async Task PerformTransactionAsync_ShouldThrowException_WhenCustomerNotFound()
    {
        // Arrange
        var transaction = new Transaction
        {
            Id = string.Empty,
            CustomerId = "1",
            Amount = 100
        };

        _customerRepositoryMock
            .Setup(r => r.GetCustomerByIdAsync(transaction.CustomerId))
            .ReturnsAsync((Customer)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ApplicationException>(() =>
            _service.PerformTransactionAsync(transaction));

        Assert.Equal("Não existe cliente com esse Id", ex.Message);

        _transactionRepositoryMock.Verify(r =>
            r.SaveTransactionAsync(It.IsAny<Transaction>()), Times.Never);

        _transactionPublisherMock.Verify(p =>
            p.PublishAsync(It.IsAny<Transaction>()), Times.Never);
    }

    [Fact]
    public async Task PerformTransactionAsync_ShouldThrowException_WhenAmountExceedsLimit()
    {
        var customerId = Guid.NewGuid().ToString();
        var transaction = new Transaction
        {
            Id = string.Empty,
            CustomerId = customerId,
            Amount = 1000
        };

        var customer = new Customer
        {
            Id = customerId,
            Name = "Um Customer",
            Cpf = "65246",
            LimitValue = 500
        };

        _customerRepositoryMock
            .Setup(r => r.GetCustomerByIdAsync(transaction.CustomerId))
            .ReturnsAsync(customer);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ApplicationException>(() =>
            _service.PerformTransactionAsync(transaction));

        Assert.Equal("Valor solicitado é superior ao limite do cliente", ex.Message);

        _transactionRepositoryMock.Verify(r =>
            r.SaveTransactionAsync(It.IsAny<Transaction>()), Times.Never);

        _transactionPublisherMock.Verify(p =>
            p.PublishAsync(It.IsAny<Transaction>()), Times.Never);
    }
}
