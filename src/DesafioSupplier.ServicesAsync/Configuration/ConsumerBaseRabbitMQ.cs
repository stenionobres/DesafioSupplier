using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace DesafioSupplier.ServicesAsync.Configuration;

public abstract class ConsumerBaseRabbitMQ : BackgroundService
{
	private readonly string _queueName;
	private readonly IOptions<ServerSettingsRabbitMQ> _rabbitServerSettings;
    private IConnection? _connection;
	private IChannel? _channel;

    protected abstract Task ConsumeMessage(string message); 

	protected ConsumerBaseRabbitMQ(string queueName, IOptions<ServerSettingsRabbitMQ> rabbitServerSettings)
	{
        _queueName = queueName;
		_rabbitServerSettings = rabbitServerSettings;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
		try
		{
			var factory = new ConnectionFactory
			{
				HostName = _rabbitServerSettings.Value.Host,
				UserName = _rabbitServerSettings.Value.User,
				Password = _rabbitServerSettings.Value.Password,
				VirtualHost = _rabbitServerSettings.Value.VirtualHost,
				RequestedConnectionTimeout = TimeSpan.FromMinutes(30),
			};

			_connection = await factory.CreateConnectionAsync(cancellationToken);
			_channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

			await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

			await base.StartAsync(cancellationToken);
		}
		catch (Exception ex)
		{
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
		if (_channel is null)
			throw new NullReferenceException("_channel is null");

		try
		{
			var queue = await _channel.QueueDeclareAsync(queue: _queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
			var consumer = new AsyncEventingBasicConsumer(_channel);

			await _channel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

			consumer.ReceivedAsync += async (model, eventArgs) =>
			{
				var body = eventArgs.Body.ToArray();
				var message = Encoding.UTF8.GetString(body);

				try
				{
					await ConsumeMessage(message);

                    // confirm processing
                    await _channel.BasicAckAsync(deliveryTag: eventArgs.DeliveryTag, multiple: false);
                }
				catch (Exception ex)
				{
                    await _channel.BasicNackAsync(eventArgs.DeliveryTag, false, true);
                    Console.WriteLine($"Erro: {ex.Message}");
                }
			};
		}
		catch (Exception ex)
		{
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
		try
		{
			if (_channel?.IsClosed is false)
				await _channel.CloseAsync(cancellationToken);

            if (_connection?.IsOpen is false)
                await _connection.CloseAsync(cancellationToken);

            await base.StopAsync(cancellationToken);
        }
		catch (Exception ex)
		{
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }
}
