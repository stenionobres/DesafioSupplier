using Microsoft.Extensions.Options;
using DesafioSupplier.ServicesAsync.Configuration;

namespace DesafioSupplier.ServicesAsync.Consumers;

public class TransactionConsumer : ConsumerBaseRabbitMQ
{
    private const string _queueName = "desafiosupplier.transaction.authorized.queue";

    public TransactionConsumer(IOptions<ServerSettingsRabbitMQ> rabbitServerSettings) : base(_queueName, rabbitServerSettings)
    {
    }

    protected override Task ConsumeMessage(string message)
    {
        throw new NotImplementedException();
    }
}
