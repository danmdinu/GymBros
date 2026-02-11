using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutApp.Domain.Entities;

namespace WorkoutApp.Infrastructure.Persistence.Configurations;

public class WorkoutPlanConfiguration : IEntityTypeConfiguration<WorkoutPlan>
{
    public void Configure(EntityTypeBuilder<WorkoutPlan> builder)
    {
        builder.HasKey(wp => wp.Id);

        builder.Property(wp => wp.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(wp => wp.Description)
            .HasMaxLength(500);

        builder.HasMany(wp => wp.Workouts)
            .WithOne()
            .HasForeignKey(w => w.WorkoutPlanId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
