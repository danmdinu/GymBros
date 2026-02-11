using System.Text.Json.Serialization;

namespace WorkoutApp.Domain.ValueObjects;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(RepBased), "reps")]
[JsonDerivedType(typeof(TimeBased), "time")]
public abstract record ExerciseMetric;

public record RepBased(int Reps) : ExerciseMetric;

public record TimeBased(int Seconds) : ExerciseMetric;
