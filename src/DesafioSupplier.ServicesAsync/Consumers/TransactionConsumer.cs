using Microsoft.Extensions.Configuration;
using DesafioSupplier.ServicesAsync.Configuration;

namespace DesafioSupplier.ServicesAsync.Consumers;

public class TransactionConsumer : ConsumerBaseRabbitMQ
{
    private const string _queueName = "desafiosupplier.transaction.authorized.queue";

    public TransactionConsumer(IConfiguration configuration) : base(_queueName)
    {
    }

    protected override Task ConsumeMessage(string message)
    {
        throw new NotImplementedException();
    }
}
