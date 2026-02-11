using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutApp.Domain.Entities;

namespace WorkoutApp.Infrastructure.Persistence.Configurations;

public class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
{
    public void Configure(EntityTypeBuilder<Workout> builder)
    {
        builder.HasKey(w => w.Id);

        builder.Property(w => w.DayNumber)
            .IsRequired();

        builder.Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(w => w.Description)
            .HasMaxLength(1000);

        builder.HasMany(w => w.Sections)
            .WithOne()
            .HasForeignKey(s => s.WorkoutId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(w => new { w.WorkoutPlanId, w.DayNumber })
            .IsUnique();
    }
}
