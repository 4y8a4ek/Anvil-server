namespace Anvil.Contracts.Dtos;

public class ExerciseDto : SyncableDto
{
    public Guid WorkoutId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
    public List<SetDto> Sets { get; set; } = new();
}