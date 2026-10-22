namespace Anvil.Contracts.Dtos;

public class ScheduleDto : SyncableDto
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<WorkoutDto> Workouts { get; set; } = new();
}

public class WorkoutDto : SyncableDto
{
    public Guid ScheduleId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public DateTime? CompletedAt { get; set; }
}