namespace DesafioSupplier.Domain.Interfaces.Publisher;

public interface IPublisher
{
    Task PublishAsync(object data);
}
