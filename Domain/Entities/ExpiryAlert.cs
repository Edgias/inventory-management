namespace EdgIQ.Inventory.Domain.Entities;

public class ExpiryAlert : BaseEntity
{
    public int BatchId { get; private set; }
    public Batch Batch { get; private set; } = default!;

    public DateTime AlertDate { get; private set; }

    public int DaysToExpiry { get; private set; }

    public AlertLevel AlertLevel { get; private set; } 

    public bool IsAcknowledged { get; private set; }

    public string? AcknowledgerId { get; private set; }
    
    public DateTime? AcknowledgedDate { get; private set; }

    public string? Action { get; private set; }

    public ExpiryAlert(int batchId, DateTime alertDate, int daysToExpiry,
        AlertLevel alertLevel)
    {
        BatchId = batchId;
        AlertDate = alertDate;
        DaysToExpiry = daysToExpiry;
        AlertLevel = alertLevel;
    }

    public void Acknowledge(string acknowledgerId, string action)
    {
        IsAcknowledged = true;
        AcknowledgerId = acknowledgerId;
        AcknowledgedDate = DateTime.UtcNow;
        Action = action;
    }

    public void Update(DateTime alertDate, int daysToExpiry, string? action)
    {
        AlertDate = alertDate;
        DaysToExpiry = daysToExpiry;
        Action = action;
    }

    public void UpdateStatus(AlertLevel alertLevel)
    {
        AlertLevel = alertLevel; 
    }
}
