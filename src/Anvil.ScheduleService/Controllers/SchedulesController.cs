using Anvil.Contracts.Dtos;
using Anvil.ScheduleService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Anvil.ScheduleService.Controllers;

[ApiController]
[Route("api/schedules")]
public class SchedulesController : ControllerBase
{
    private readonly AppDbContext _db;

    public SchedulesController(AppDbContext db) => _db = db;

    [HttpGet("sync")]
    public async Task<ActionResult<List<ScheduleDto>>> GetForSync(
        [FromQuery] Guid userId,
        [FromQuery] DateTime? since)
    {
        var query = _db.Schedules
            .Include(s => s.Workouts)
            .Where(s => s.UserId == userId && !s.IsDeleted);

        if (since.HasValue)
            query = query.Where(s => s.UpdatedAt > since.Value);

        var schedules = await query.ToListAsync();

        var dtos = schedules.Select(s => new ScheduleDto
        {
            Id = s.Id,
            UserId = s.UserId,
            Title = s.Title,
            UpdatedAt = s.UpdatedAt,
            IsDeleted = s.IsDeleted,
            Workouts = s.Workouts
                .Where(w => !w.IsDeleted)
                .Select(w => new WorkoutDto
                {
                    Id = w.Id,
                    ScheduleId = w.ScheduleId,
                    Title = w.Title,
                    ScheduledDate = w.ScheduledDate,
                    CompletedAt = w.CompletedAt,
                    UpdatedAt = w.UpdatedAt,
                    IsDeleted = w.IsDeleted
                }).ToList()
        }).ToList();

        return Ok(dtos);
    }
}