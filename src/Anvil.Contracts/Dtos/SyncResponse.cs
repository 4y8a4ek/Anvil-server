namespace Anvil.Contracts.Dtos;

public class SyncResponse
{
    public DateTime CurrentServerTimestamp { get; set; }
    public List<ScheduleDto> Schedules { get; set; } = new();
    public List<ExerciseDto> Exercises { get; set; } = new();
}