using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using DesafioSupplier.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DesafioSupplier.Domain.Interfaces.Services;
using DesafioSupplier.ServicesAsync.Configuration;

namespace DesafioSupplier.ServicesAsync.Consumers;

public class TransactionConsumer : ConsumerBaseRabbitMQ
{
    private readonly ILogger<TransactionConsumer> _logger;
    private readonly IServiceProvider _serviceProvider;

    public TransactionConsumer(IConfiguration configuration,
                               ILogger<TransactionConsumer> logger,
                               IServiceProvider serviceProvider,
                               IOptions<ServerSettingsRabbitMQ> rabbitServerSettings) 
                               : base(configuration["TransactionSettings:QueueName"] ?? string.Empty, rabbitServerSettings)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ConsumeMessage(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            _logger.LogError($"{nameof(TransactionConsumer)} - Mensagem recebia vazia ou nula");
            return;
        }

        var transaction = JsonSerializer.Deserialize<Transaction>(message);

        if (transaction == null)
        {
            _logger.LogError($"{nameof(TransactionConsumer)} - Falha ao deserializar a transaction message");
            return;
        }

        using (var scope = _serviceProvider.CreateScope())
        {
            var customerService = scope.ServiceProvider.GetRequiredService<ICustomerService>();
            await customerService.UpdateLimit(transaction.CustomerId, transaction.Amount);
        }
    }
}
