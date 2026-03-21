using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using DesafioSupplier.ServicesAsync.Configuration;

namespace DesafioSupplier.ServicesAsync.Consumers;

public class TransactionConsumer : ConsumerBaseRabbitMQ
{
    public TransactionConsumer(IConfiguration configuration, IOptions<ServerSettingsRabbitMQ> rabbitServerSettings) 
                        : base(configuration["TransactionSettings:QueueName"] ?? string.Empty, rabbitServerSettings)
    {
    }

    protected override Task ConsumeMessage(string message)
    {
        return Task.CompletedTask ;
    }
}
