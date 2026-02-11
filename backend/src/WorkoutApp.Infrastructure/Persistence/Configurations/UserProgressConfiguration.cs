using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutApp.Domain.Entities;

namespace WorkoutApp.Infrastructure.Persistence.Configurations;

public class UserProgressConfiguration : IEntityTypeConfiguration<UserProgress>
{
    public void Configure(EntityTypeBuilder<UserProgress> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.UserId)
            .IsRequired();

        builder.HasIndex(p => p.UserId)
            .IsUnique();

        builder.Property(p => p.CurrentDayNumber)
            .IsRequired();

        builder.Property(p => p.StartedAt)
            .IsRequired();

        builder.Ignore(p => p.IsCompleted);
    }
}
