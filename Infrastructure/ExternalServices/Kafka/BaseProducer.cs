#nullable disable

using Application;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Newtonsoft.Json;

namespace Infrastructure.ExternalServices.Kafka;

public abstract class BaseProducer<T> : IProducerHandler<T>, IDisposable
{
    private readonly IProducer<string, string> _producer;
    private readonly string _topic;

    protected BaseProducer(AppSettings appSettings, string topicName)
    {
        var kafkaConfig = appSettings.KafkaConfig;

        _producer = new ProducerBuilder<string, string>(new ProducerConfig
        {
            BootstrapServers = kafkaConfig.BootstrapServers
        }).Build();

        _topic = $"{kafkaConfig.TopicPrefix}{topicName}";

        EnsureTopicExists(kafkaConfig.BootstrapServers, _topic);
    }

    private static void EnsureTopicExists(string bootstrapServers, string topic)
    {
        using var adminClient = new AdminClientBuilder(new AdminClientConfig
        {
            BootstrapServers = bootstrapServers
        }).Build();

        try
        {
            adminClient.CreateTopicsAsync(new[]
            {
                new TopicSpecification
                {
                    Name = topic,
                    NumPartitions = 1,
                    ReplicationFactor = 1
                }
            }).GetAwaiter().GetResult();
        }
        catch (CreateTopicsException ex)
            when (ex.Results.All(r => r.Error.Code == ErrorCode.TopicAlreadyExists))
        {
            // topic already exists — safe to continue
        }
    }

    public async Task ProduceAsync(T message, CancellationToken cancellationToken = default)
    {
        var json = JsonConvert.SerializeObject(message);

        await _producer.ProduceAsync(_topic, new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = json
        }, cancellationToken);
    }

    public void Dispose()
    {
        _producer.Flush(TimeSpan.FromSeconds(5));
        _producer.Dispose();
    }
}
