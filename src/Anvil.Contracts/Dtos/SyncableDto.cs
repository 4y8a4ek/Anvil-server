namespace Anvil.Contracts.Dtos;

public abstract class SyncableDto
{
    public Guid Id { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}