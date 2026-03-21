using System.Text;
using RabbitMQ.Client;
using Microsoft.Extensions.Options;

namespace DesafioSupplier.ServicesAsync.Configuration;

public class PublisherBaseRabbitMQ
{
    private readonly IOptions<ServerSettingsRabbitMQ> _rabbitServerSettings;

    public PublisherBaseRabbitMQ(IOptions<ServerSettingsRabbitMQ> rabbitServerSettings)
    {
        _rabbitServerSettings = rabbitServerSettings;
    }

    public async Task PublishAsync(string queue, string messageAsJson)
    {
        var factory = new ConnectionFactory
        {
            HostName = _rabbitServerSettings.Value.Host,
            UserName = _rabbitServerSettings.Value.User,
            Password = _rabbitServerSettings.Value.Password,
            VirtualHost = _rabbitServerSettings.Value.VirtualHost,
            RequestedConnectionTimeout = TimeSpan.FromMinutes(30),
        };

        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();
        var props = new BasicProperties { Persistent = true };
        var body = Encoding.UTF8.GetBytes(messageAsJson);

        await channel.BasicPublishAsync(exchange: string.Empty, queue, mandatory: true, props, body);
    }
}
