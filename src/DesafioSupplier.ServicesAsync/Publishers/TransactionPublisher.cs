using System.Text.Json;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using DesafioSupplier.ServicesAsync.Configuration;

namespace DesafioSupplier.ServicesAsync.Publishers;

public class TransactionPublisher : PublisherBaseRabbitMQ
{
    private readonly string _queueName;

    public TransactionPublisher(IConfiguration configuration, IOptions<ServerSettingsRabbitMQ> rabbitServerSettings) : base(rabbitServerSettings)
    {
        _queueName = configuration["TransactionSettings:QueueName"] ?? string.Empty;
    }

    public async Task PublishAsync(object data)
    {
        var dataAsJson = JsonSerializer.Serialize(data);
        await PublishAsync(_queueName, dataAsJson);
    }
}
