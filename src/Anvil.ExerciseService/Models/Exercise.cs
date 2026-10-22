namespace Anvil.ExerciseService.Models;

public class Exercise
{
    public Guid Id { get; set; }
    public Guid WorkoutId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
    public string? Notes { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public List<Set> Sets { get; set; } = new();
}