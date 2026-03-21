namespace DesafioSupplier.ServicesAsync.Configuration;

public class ServerSettingsRabbitMQ
{
    public required string Host { get; set; }
    public required string User { get; set; }
    public required string Password { get; set; }
    public required string VirtualHost { get; set; }
}
