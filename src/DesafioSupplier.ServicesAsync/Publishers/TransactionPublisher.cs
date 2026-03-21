using System.Text.Json;
using Microsoft.Extensions.Options;
using DesafioSupplier.ServicesAsync.Configuration;

namespace DesafioSupplier.ServicesAsync.Publishers;

public class TransactionPublisher : PublisherBaseRabbitMQ
{
    private const string _queueName = "desafiosupplier.transaction.authorized.queue";

    public TransactionPublisher(IOptions<ServerSettingsRabbitMQ> rabbitServerSettings) : base(rabbitServerSettings)
    {
    }

    public async Task PublishAsync(object data)
    {
        var dataAsJson = JsonSerializer.Serialize(data);
        await PublishAsync(_queueName, dataAsJson);
    }
}
