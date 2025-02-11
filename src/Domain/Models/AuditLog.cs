namespace Domain.Models;

public class AuditLog
{
    public int Id { get; set; }
    public string TableName { get; set; }
    public Guid RecordId { get; set; }
    public string ActionType { get; set; }
    public string UserName { get; set; }
    public DateTime ChangedAt { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
}