#nullable disable

namespace Application.DTOs.Common.Config;

public class KafkaConfigDTO
{
    public string BootstrapServers { get; set; }
    public string TopicPrefix { get; set; }
    public string GroupId { get; set; }
}
