using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutApp.Domain.Entities;

namespace WorkoutApp.Infrastructure.Persistence.Configurations;

public class WorkoutSectionConfiguration : IEntityTypeConfiguration<WorkoutSection>
{
    public void Configure(EntityTypeBuilder<WorkoutSection> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Order)
            .IsRequired();

        builder.Property(s => s.Type)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(s => s.Exercises)
            .WithOne()
            .HasForeignKey(e => e.WorkoutSectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
