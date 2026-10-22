using Anvil.ScheduleService.Models;
using Microsoft.EntityFrameworkCore;

namespace Anvil.ScheduleService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Schedule> Schedules => Set<Schedule>();
    public DbSet<Workout> Workouts => Set<Workout>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Schedule>()
            .HasMany(s => s.Workouts)
            .WithOne(w => w.Schedule)
            .HasForeignKey(w => w.ScheduleId);
    }
}