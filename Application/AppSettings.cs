#nullable disable

using Application.DTOs.Common.Config;
using Microsoft.Extensions.Configuration;

namespace Application;

public class AppSettings
{
    private readonly IConfiguration _configuration;

    public AppSettings(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public KafkaConfigDTO KafkaConfig => _configuration.GetSection("Kafka").Get<KafkaConfigDTO>();
}
