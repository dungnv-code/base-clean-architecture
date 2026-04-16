#nullable disable

namespace Application.DTOs.Kafka;

public class AuditLogDTO
{
    public string Action { get; set; }
    public string Entity { get; set; }
    public string EntityId { get; set; }
    public string UserId { get; set; }
    public string Detail { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
