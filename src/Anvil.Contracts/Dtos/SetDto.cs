using System.Text.Json.Serialization;

namespace Anvil.Contracts.Dtos;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "setType")]
[JsonDerivedType(typeof(RepsSetDto), "Reps")]
[JsonDerivedType(typeof(TimeSetDto), "Time")]
[JsonDerivedType(typeof(VersatileSetDto), "Versatile")]
public abstract class SetDto : SyncableDto
{
    public Guid ExerciseId { get; set; }
    public int Order { get; set; }
}

public class RepsSetDto : SetDto
{
    public int Repetitions { get; set; }
    public float? Weight { get; set; }
}

public class TimeSetDto : SetDto
{
    public int DurationSeconds { get; set; }
}

public class VersatileSetDto : SetDto
{
    public int? Repetitions { get; set; }
    public float? Weight { get; set; }
    public int? DurationSeconds { get; set; }
    public float? DistanceMeters { get; set; }
}