using Anvil.ExerciseService.Models;
using Microsoft.EntityFrameworkCore;

namespace Anvil.ExerciseService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<Set> Sets => Set<Set>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // TPH-дискриминатор
        modelBuilder.Entity<Set>()
            .HasDiscriminator<string>("SetType")
            .HasValue<RepsSet>("Reps")
            .HasValue<TimeSet>("Time")
            .HasValue<VersatileSet>("Versatile");

        // Индексы для синхронизации и запросов
        modelBuilder.Entity<Set>()
            .HasIndex(s => new { s.ExerciseId, s.UpdatedAt });

        modelBuilder.Entity<Exercise>()
            .HasIndex(e => e.UpdatedAt);

        modelBuilder.Entity<Exercise>()
            .HasMany(e => e.Sets)
            .WithOne(s => s.Exercise)
            .HasForeignKey(s => s.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}