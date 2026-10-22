namespace Anvil.ExerciseService.Models;

public abstract class Set
{
    public Guid Id { get; set; }
    public Guid ExerciseId { get; set; }
    public Exercise Exercise { get; set; } = null!;
    public int Order { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}

public class RepsSet : Set
{
    public int Repetitions { get; set; }
    public float? Weight { get; set; }
}

public class TimeSet : Set
{
    public int DurationSeconds { get; set; }
}

public class VersatileSet : Set
{
    public int? Repetitions { get; set; }
    public float? Weight { get; set; }
    public int? DurationSeconds { get; set; }
    public float? DistanceMeters { get; set; }
}