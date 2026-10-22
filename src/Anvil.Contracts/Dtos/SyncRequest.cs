namespace Anvil.Contracts.Dtos;

public class SyncRequest
{
    public Guid UserId { get; set; }
    public DateTime? LastSyncTimestamp { get; set; }
}