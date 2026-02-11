using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WorkoutApp.Domain.Entities;
using WorkoutApp.Domain.ValueObjects;

namespace WorkoutApp.Infrastructure.Persistence.Configurations;

public class WorkoutExerciseConfiguration : IEntityTypeConfiguration<WorkoutExercise>
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public void Configure(EntityTypeBuilder<WorkoutExercise> builder)
    {
        builder.HasKey(we => we.Id);

        builder.Property(we => we.Order)
            .IsRequired();

        builder.Property(we => we.SupersetGroup)
            .HasMaxLength(10);

        builder.Property(we => we.Sets)
            .IsRequired();

        builder.Property(we => we.Notes)
            .HasMaxLength(500);

        // Store ExerciseMetric as JSON string with custom converter
        builder.Property(we => we.Metric)
            .HasConversion(
                v => JsonSerializer.Serialize(v, JsonOptions),
                v => JsonSerializer.Deserialize<ExerciseMetric>(v, JsonOptions)!)
            .HasColumnType("TEXT")
            .IsRequired();

        builder.HasOne(we => we.Exercise)
            .WithMany()
            .HasForeignKey(we => we.ExerciseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
