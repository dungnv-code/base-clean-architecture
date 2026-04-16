#nullable disable

using Application;
using Application.DTOs.Kafka;
using Domain.Constants;

namespace Infrastructure.ExternalServices.Kafka;

public class AuditLogProducerHandler : BaseProducer<AuditLogDTO>
{
    public AuditLogProducerHandler(AppSettings appSettings)
        : base(appSettings, KafkaTopicConstant.AuditLog)
    {
    }
}
