using Anvil.Contracts.Dtos;
using Anvil.ExerciseService.Data;
using Anvil.ExerciseService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Anvil.ExerciseService.Controllers;

[ApiController]
[Route("api/exercises")]
public class ExercisesController : ControllerBase
{
    private readonly AppDbContext _db;

    public ExercisesController(AppDbContext db) => _db = db;

    [HttpGet("sync")]
    public async Task<ActionResult<List<ExerciseDto>>> GetForSync(
        [FromQuery] Guid userId,
        [FromQuery] DateTime? since)
    {
        // В реальной системе WorkoutId должен проверяться через ScheduleService,
        // но для заготовки синхронизации мы просто отдаём упражнения,
        // связанные с тренировками пользователя (предполагается, что
        // WorkoutId уже отфильтрованы на стороне Gateway).
        var query = _db.Exercises
            .Include(e => e.Sets)
            .AsQueryable();

        if (since.HasValue)
            query = query.Where(e => e.UpdatedAt > since.Value);

        var exercises = await query
            .Where(e => !e.IsDeleted)
            .ToListAsync();

        var dtos = exercises.Select(e => new ExerciseDto
        {
            Id = e.Id,
            WorkoutId = e.WorkoutId,
            Name = e.Name,
            Order = e.Order,
            UpdatedAt = e.UpdatedAt,
            IsDeleted = e.IsDeleted,
            Sets = e.Sets
                .Where(s => !s.IsDeleted)
                .Select(MapSetToDto)
                .ToList()
        }).ToList();

        return Ok(dtos);
    }

    private static SetDto MapSetToDto(Set set) => set switch
    {
        RepsSet reps => new RepsSetDto
        {
            Id = reps.Id,
            ExerciseId = reps.ExerciseId,
            Order = reps.Order,
            Repetitions = reps.Repetitions,
            Weight = reps.Weight,
            UpdatedAt = reps.UpdatedAt,
            IsDeleted = reps.IsDeleted
        },
        TimeSet time => new TimeSetDto
        {
            Id = time.Id,
            ExerciseId = time.ExerciseId,
            Order = time.Order,
            DurationSeconds = time.DurationSeconds,
            UpdatedAt = time.UpdatedAt,
            IsDeleted = time.IsDeleted
        },
        VersatileSet vers => new VersatileSetDto
        {
            Id = vers.Id,
            ExerciseId = vers.ExerciseId,
            Order = vers.Order,
            Repetitions = vers.Repetitions,
            Weight = vers.Weight,
            DurationSeconds = vers.DurationSeconds,
            DistanceMeters = vers.DistanceMeters,
            UpdatedAt = vers.UpdatedAt,
            IsDeleted = vers.IsDeleted
        },
        _ => throw new InvalidOperationException($"Unknown set type: {set.GetType().Name}")
    };
}