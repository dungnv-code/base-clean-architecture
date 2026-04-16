#nullable disable

namespace Infrastructure.ExternalServices.Kafka;

public interface IProducerHandler<T>
{
    Task ProduceAsync(T message, CancellationToken cancellationToken = default);
}
