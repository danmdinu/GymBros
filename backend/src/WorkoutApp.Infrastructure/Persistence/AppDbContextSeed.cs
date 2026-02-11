using Microsoft.EntityFrameworkCore;
using WorkoutApp.Domain.Entities;
using WorkoutApp.Domain.Enums;
using WorkoutApp.Domain.ValueObjects;

namespace WorkoutApp.Infrastructure.Persistence;

public static class AppDbContextSeed
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await SeedExercisesAsync(context);
        await SeedWorkoutPlanAsync(context);
    }

    private static async Task SeedExercisesAsync(AppDbContext context)
    {
        if (await context.Exercises.AnyAsync())
            return;

        var exercises = new List<Exercise>
        {
            new("Push-ups", "Keep your core tight, lower your chest to the ground, then push back up."),
            new("Squats", "Stand with feet shoulder-width apart, lower your hips as if sitting in a chair."),
            new("Plank", "Hold a push-up position with your body in a straight line."),
            new("Jumping Jacks", "Jump while spreading legs and raising arms overhead."),
            new("Mountain Climbers", "In plank position, alternate driving knees toward chest."),
            new("Burpees", "Drop to push-up, perform push-up, jump up with arms overhead."),
            new("Lunges", "Step forward and lower until both knees are at 90 degrees."),
            new("High Knees", "Run in place, bringing knees up to hip level."),
            new("Tricep Dips", "Using a chair or bench, lower and raise your body using your arms."),
            new("Bicycle Crunches", "Lie on back, alternate touching elbow to opposite knee."),
            new("Diamond Push-ups", "Push-ups with hands close together forming a diamond shape."),
            new("Jump Squats", "Perform a squat, then explosively jump up."),
            new("Side Plank", "Hold plank position on your side, supporting with one arm."),
            new("Leg Raises", "Lie on back, raise legs to 90 degrees, lower slowly."),
            new("Russian Twists", "Seated, lean back slightly, rotate torso side to side."),
            new("Wall Sit", "Lean against wall with knees at 90 degrees, hold position."),
            new("Calf Raises", "Stand on toes, raise heels off ground, lower slowly."),
            new("Superman", "Lie face down, raise arms and legs off ground simultaneously."),
            new("Glute Bridges", "Lie on back, knees bent, raise hips toward ceiling."),
            new("Box Jumps", "Jump onto a raised platform, step down, repeat.")
        };

        context.Exercises.AddRange(exercises);
        await context.SaveChangesAsync();
    }

    private static async Task SeedWorkoutPlanAsync(AppDbContext context)
    {
        if (await context.WorkoutPlans.AnyAsync())
            return;

        var exercises = await context.Exercises.ToDictionaryAsync(e => e.Name);
        var plan = new WorkoutPlan("30-Day Challenge", "A 30-day full body workout program. Commit to daily movement!");

        // Week 1: Foundation
        plan.AddWorkout(BuildDay1(exercises));
        plan.AddWorkout(BuildDay2(exercises));
        plan.AddWorkout(BuildDay3(exercises));
        plan.AddWorkout(BuildDay4(exercises));
        plan.AddWorkout(BuildDay5(exercises));
        plan.AddWorkout(BuildDay6(exercises));
        plan.AddWorkout(BuildDay7(exercises));

        // Week 2: Building
        plan.AddWorkout(BuildLegsDay(8, "Day 8: Legs & Glutes", "Week 2 begins! Time to level up.", exercises));
        plan.AddWorkout(BuildUpperDay(9, "Day 9: Upper Body Blast", "Push your limits today!", exercises));
        plan.AddWorkout(BuildCoreDay(10, "Day 10: Core & Cardio", "Burn calories and build abs.", exercises));
        plan.AddWorkout(BuildHIITDay(11, "Day 11: Full Body HIIT", "High intensity interval training!", exercises));
        plan.AddWorkout(BuildLegsDay(12, "Day 12: Lower Body Strength", "Strong legs, strong foundation.", exercises));
        plan.AddWorkout(BuildPushCoreDay(13, "Day 13: Push & Core", "Chest, triceps, and abs.", exercises));
        plan.AddWorkout(BuildRecoveryDay(14, "Day 14: Active Recovery", "Week 2 done! Light movement.", exercises));

        // Week 3: Peak
        plan.AddWorkout(BuildLegsDay(15, "Day 15: Leg Day Extreme", "Week 3 - Peak week begins!", exercises));
        plan.AddWorkout(BuildUpperDay(16, "Day 16: Push to the Max", "Maximum effort today!", exercises));
        plan.AddWorkout(BuildCoreDay(17, "Day 17: Ab Destroyer", "Six-pack in progress.", exercises));
        plan.AddWorkout(BuildHIITDay(18, "Day 18: Total Body Burn", "Everything burns today.", exercises));
        plan.AddWorkout(BuildLegsDay(19, "Day 19: Glute Focus", "Build that booty!", exercises));
        plan.AddWorkout(BuildUpperDay(20, "Day 20: Upper Power", "Strong arms, strong you.", exercises));
        plan.AddWorkout(BuildRecoveryDay(21, "Day 21: Rest & Reflect", "3 weeks down! Celebrate.", exercises));

        // Week 4: Consolidation
        plan.AddWorkout(BuildLegsDay(22, "Day 22: Leg Sculpt", "Final week - stay strong!", exercises));
        plan.AddWorkout(BuildPushCoreDay(23, "Day 23: Chest & Tris", "Push it!", exercises));
        plan.AddWorkout(BuildCoreDay(24, "Day 24: Core Finisher", "Almost there!", exercises));
        plan.AddWorkout(BuildHIITDay(25, "Day 25: HIIT Blast", "Cardio and strength.", exercises));
        plan.AddWorkout(BuildLegsDay(26, "Day 26: Lower Body Last", "Final leg day!", exercises));
        plan.AddWorkout(BuildUpperDay(27, "Day 27: Upper Body Final", "Last push workout!", exercises));
        plan.AddWorkout(BuildRecoveryDay(28, "Day 28: Light Day", "Prepare for the finale.", exercises));

        // Finale
        plan.AddWorkout(BuildHIITDay(29, "Day 29: Ultimate Challenge", "Give it everything!", exercises));
        plan.AddWorkout(BuildRecoveryDay(30, "Day 30: Victory Lap", "YOU DID IT! Celebrate with movement.", exercises));

        context.WorkoutPlans.Add(plan);
        await context.SaveChangesAsync();
    }

    // Helper methods for creating metrics
    private static ExerciseMetric Reps(int reps) => new RepBased(reps);
    private static ExerciseMetric Time(int seconds) => new TimeBased(seconds);

    // ===== WEEK 1 DETAILED DAYS =====

    private static Workout BuildDay1(Dictionary<string, Exercise> ex)
    {
        var workout = new Workout(1, "Day 1: Upper Body Intro", "Welcome! Focus on form. Rest 30-60s between exercises.");
        
        var warmup = new WorkoutSection(1, SectionType.WarmUp, "Warm Up");
        warmup.AddExercise(new WorkoutExercise(1, ex["Jumping Jacks"].Id, 1, Reps(30)));
        warmup.AddExercise(new WorkoutExercise(2, ex["High Knees"].Id, 1, Time(30)));
        workout.AddSection(warmup);

        var main = new WorkoutSection(2, SectionType.Main, "Main Workout");
        main.AddExercise(new WorkoutExercise(1, ex["Push-ups"].Id, 3, Reps(10), "A", "Keep core tight"));
        main.AddExercise(new WorkoutExercise(2, ex["Plank"].Id, 3, Time(30), "A"));
        main.AddExercise(new WorkoutExercise(3, ex["Mountain Climbers"].Id, 3, Reps(20), "A"));
        main.AddExercise(new WorkoutExercise(4, ex["Burpees"].Id, 3, Reps(8)));
        workout.AddSection(main);

        var cooldown = new WorkoutSection(3, SectionType.CoolDown, "Cool Down");
        cooldown.AddExercise(new WorkoutExercise(1, ex["Plank"].Id, 1, Time(60), notes: "Hold and breathe"));
        workout.AddSection(cooldown);

        return workout;
    }

    private static Workout BuildDay2(Dictionary<string, Exercise> ex)
    {
        var workout = new Workout(2, "Day 2: Lower Body Power", "Focus on leg strength. Rest 45s between sets.");
        
        var warmup = new WorkoutSection(1, SectionType.WarmUp, "Warm Up");
        warmup.AddExercise(new WorkoutExercise(1, ex["Jumping Jacks"].Id, 1, Reps(30)));
        warmup.AddExercise(new WorkoutExercise(2, ex["High Knees"].Id, 1, Time(30)));
        workout.AddSection(warmup);

        var main = new WorkoutSection(2, SectionType.Main, "Main Workout");
        main.AddExercise(new WorkoutExercise(1, ex["Squats"].Id, 4, Reps(15)));
        main.AddExercise(new WorkoutExercise(2, ex["Lunges"].Id, 3, Reps(12), notes: "12 each leg"));
        main.AddExercise(new WorkoutExercise(3, ex["Wall Sit"].Id, 3, Time(30)));
        main.AddExercise(new WorkoutExercise(4, ex["Calf Raises"].Id, 3, Reps(20)));
        workout.AddSection(main);

        var cooldown = new WorkoutSection(3, SectionType.CoolDown, "Cool Down");
        cooldown.AddExercise(new WorkoutExercise(1, ex["Glute Bridges"].Id, 2, Reps(15), notes: "Squeeze at top"));
        workout.AddSection(cooldown);

        return workout;
    }

    private static Workout BuildDay3(Dictionary<string, Exercise> ex)
    {
        var workout = new Workout(3, "Day 3: Core Crusher", "Build that core! Keep abs engaged.");
        
        var warmup = new WorkoutSection(1, SectionType.WarmUp, "Warm Up");
        warmup.AddExercise(new WorkoutExercise(1, ex["Jumping Jacks"].Id, 1, Reps(25)));
        warmup.AddExercise(new WorkoutExercise(2, ex["Mountain Climbers"].Id, 1, Reps(20)));
        workout.AddSection(warmup);

        var main = new WorkoutSection(2, SectionType.Main, "Main Workout");
        main.AddExercise(new WorkoutExercise(1, ex["Plank"].Id, 3, Time(45)));
        main.AddExercise(new WorkoutExercise(2, ex["Bicycle Crunches"].Id, 3, Reps(20)));
        main.AddExercise(new WorkoutExercise(3, ex["Leg Raises"].Id, 3, Reps(12)));
        main.AddExercise(new WorkoutExercise(4, ex["Russian Twists"].Id, 3, Reps(20), notes: "10 each side"));
        main.AddExercise(new WorkoutExercise(5, ex["Superman"].Id, 3, Reps(15)));
        workout.AddSection(main);

        var cooldown = new WorkoutSection(3, SectionType.CoolDown, "Cool Down");
        cooldown.AddExercise(new WorkoutExercise(1, ex["Side Plank"].Id, 2, Time(20), notes: "20 sec each side"));
        workout.AddSection(cooldown);

        return workout;
    }

    private static Workout BuildDay4(Dictionary<string, Exercise> ex)
    {
        var workout = new Workout(4, "Day 4: Active Recovery", "Light day! Focus on mobility.");
        
        var warmup = new WorkoutSection(1, SectionType.WarmUp, "Warm Up");
        warmup.AddExercise(new WorkoutExercise(1, ex["Jumping Jacks"].Id, 1, Reps(20), notes: "Easy pace"));
        workout.AddSection(warmup);

        var main = new WorkoutSection(2, SectionType.Main, "Main Workout");
        main.AddExercise(new WorkoutExercise(1, ex["Squats"].Id, 2, Reps(10), notes: "Slow and controlled"));
        main.AddExercise(new WorkoutExercise(2, ex["Lunges"].Id, 2, Reps(8), notes: "8 each leg"));
        main.AddExercise(new WorkoutExercise(3, ex["Glute Bridges"].Id, 2, Reps(12)));
        main.AddExercise(new WorkoutExercise(4, ex["Plank"].Id, 2, Time(30)));
        workout.AddSection(main);

        var cooldown = new WorkoutSection(3, SectionType.CoolDown, "Cool Down");
        cooldown.AddExercise(new WorkoutExercise(1, ex["Superman"].Id, 1, Reps(10), notes: "Hold each 2 sec"));
        workout.AddSection(cooldown);

        return workout;
    }

    private static Workout BuildDay5(Dictionary<string, Exercise> ex)
    {
        var workout = new Workout(5, "Day 5: Full Body Burn", "Let's pick up the intensity!");
        
        var warmup = new WorkoutSection(1, SectionType.WarmUp, "Warm Up");
        warmup.AddExercise(new WorkoutExercise(1, ex["Jumping Jacks"].Id, 1, Reps(30)));
        warmup.AddExercise(new WorkoutExercise(2, ex["High Knees"].Id, 1, Time(30)));
        warmup.AddExercise(new WorkoutExercise(3, ex["Mountain Climbers"].Id, 1, Reps(15)));
        workout.AddSection(warmup);

        var main = new WorkoutSection(2, SectionType.Main, "Main Workout");
        main.AddExercise(new WorkoutExercise(1, ex["Burpees"].Id, 4, Reps(10)));
        main.AddExercise(new WorkoutExercise(2, ex["Push-ups"].Id, 3, Reps(12)));
        main.AddExercise(new WorkoutExercise(3, ex["Jump Squats"].Id, 3, Reps(15)));
        main.AddExercise(new WorkoutExercise(4, ex["Plank"].Id, 3, Time(45)));
        main.AddExercise(new WorkoutExercise(5, ex["Bicycle Crunches"].Id, 3, Reps(20)));
        workout.AddSection(main);

        var cooldown = new WorkoutSection(3, SectionType.CoolDown, "Cool Down");
        cooldown.AddExercise(new WorkoutExercise(1, ex["Glute Bridges"].Id, 1, Reps(15)));
        workout.AddSection(cooldown);

        return workout;
    }

    private static Workout BuildDay6(Dictionary<string, Exercise> ex)
    {
        var workout = new Workout(6, "Day 6: Push Power", "Upper body push focus. Feel the burn!");
        
        var warmup = new WorkoutSection(1, SectionType.WarmUp, "Warm Up");
        warmup.AddExercise(new WorkoutExercise(1, ex["Jumping Jacks"].Id, 1, Reps(25)));
        warmup.AddExercise(new WorkoutExercise(2, ex["Push-ups"].Id, 1, Reps(5), notes: "Warm-up set"));
        workout.AddSection(warmup);

        var main = new WorkoutSection(2, SectionType.Main, "Main Workout");
        main.AddExercise(new WorkoutExercise(1, ex["Push-ups"].Id, 4, Reps(12)));
        main.AddExercise(new WorkoutExercise(2, ex["Diamond Push-ups"].Id, 3, Reps(8), notes: "Harder variation"));
        main.AddExercise(new WorkoutExercise(3, ex["Tricep Dips"].Id, 3, Reps(12)));
        main.AddExercise(new WorkoutExercise(4, ex["Plank"].Id, 3, Time(40)));
        workout.AddSection(main);

        var cooldown = new WorkoutSection(3, SectionType.CoolDown, "Cool Down");
        cooldown.AddExercise(new WorkoutExercise(1, ex["Superman"].Id, 2, Reps(12)));
        workout.AddSection(cooldown);

        return workout;
    }

    private static Workout BuildDay7(Dictionary<string, Exercise> ex)
    {
        var workout = new Workout(7, "Day 7: Stretch & Restore", "Week 1 complete! Light stretching.");
        
        var warmup = new WorkoutSection(1, SectionType.WarmUp, "Warm Up");
        warmup.AddExercise(new WorkoutExercise(1, ex["Jumping Jacks"].Id, 1, Reps(15), notes: "Light and easy"));
        workout.AddSection(warmup);

        var main = new WorkoutSection(2, SectionType.Main, "Main Workout");
        main.AddExercise(new WorkoutExercise(1, ex["Glute Bridges"].Id, 2, Reps(10), notes: "Hold 3 sec at top"));
        main.AddExercise(new WorkoutExercise(2, ex["Side Plank"].Id, 2, Time(20), notes: "Each side"));
        main.AddExercise(new WorkoutExercise(3, ex["Superman"].Id, 2, Reps(10), notes: "Hold 2 sec"));
        main.AddExercise(new WorkoutExercise(4, ex["Plank"].Id, 1, Time(60), notes: "Final hold"));
        workout.AddSection(main);

        var cooldown = new WorkoutSection(3, SectionType.CoolDown, "Cool Down");
        cooldown.AddExercise(new WorkoutExercise(1, ex["Leg Raises"].Id, 1, Reps(10), notes: "Slow and controlled"));
        workout.AddSection(cooldown);

        return workout;
    }

    // ===== REUSABLE WORKOUT TEMPLATES =====

    private static Workout BuildLegsDay(int day, string name, string description, Dictionary<string, Exercise> ex)
    {
        var workout = new Workout(day, name, description);
        
        var warmup = new WorkoutSection(1, SectionType.WarmUp, "Warm Up");
        warmup.AddExercise(new WorkoutExercise(1, ex["Jumping Jacks"].Id, 1, Reps(30)));
        warmup.AddExercise(new WorkoutExercise(2, ex["High Knees"].Id, 1, Time(30)));
        workout.AddSection(warmup);

        var main = new WorkoutSection(2, SectionType.Main, "Main Workout");
        main.AddExercise(new WorkoutExercise(1, ex["Squats"].Id, 4, Reps(15)));
        main.AddExercise(new WorkoutExercise(2, ex["Lunges"].Id, 3, Reps(12), notes: "Each leg"));
        main.AddExercise(new WorkoutExercise(3, ex["Jump Squats"].Id, 3, Reps(10)));
        main.AddExercise(new WorkoutExercise(4, ex["Wall Sit"].Id, 3, Time(45)));
        main.AddExercise(new WorkoutExercise(5, ex["Glute Bridges"].Id, 3, Reps(15)));
        workout.AddSection(main);

        var cooldown = new WorkoutSection(3, SectionType.CoolDown, "Cool Down");
        cooldown.AddExercise(new WorkoutExercise(1, ex["Calf Raises"].Id, 2, Reps(20)));
        workout.AddSection(cooldown);

        return workout;
    }

    private static Workout BuildUpperDay(int day, string name, string description, Dictionary<string, Exercise> ex)
    {
        var workout = new Workout(day, name, description);
        
        var warmup = new WorkoutSection(1, SectionType.WarmUp, "Warm Up");
        warmup.AddExercise(new WorkoutExercise(1, ex["Jumping Jacks"].Id, 1, Reps(25)));
        warmup.AddExercise(new WorkoutExercise(2, ex["Push-ups"].Id, 1, Reps(5), notes: "Warm-up"));
        workout.AddSection(warmup);

        var main = new WorkoutSection(2, SectionType.Main, "Main Workout");
        main.AddExercise(new WorkoutExercise(1, ex["Push-ups"].Id, 4, Reps(15)));
        main.AddExercise(new WorkoutExercise(2, ex["Diamond Push-ups"].Id, 3, Reps(10)));
        main.AddExercise(new WorkoutExercise(3, ex["Tricep Dips"].Id, 4, Reps(12)));
        main.AddExercise(new WorkoutExercise(4, ex["Plank"].Id, 3, Time(45)));
        workout.AddSection(main);

        var cooldown = new WorkoutSection(3, SectionType.CoolDown, "Cool Down");
        cooldown.AddExercise(new WorkoutExercise(1, ex["Superman"].Id, 2, Reps(15)));
        workout.AddSection(cooldown);

        return workout;
    }

    private static Workout BuildCoreDay(int day, string name, string description, Dictionary<string, Exercise> ex)
    {
        var workout = new Workout(day, name, description);
        
        var warmup = new WorkoutSection(1, SectionType.WarmUp, "Warm Up");
        warmup.AddExercise(new WorkoutExercise(1, ex["High Knees"].Id, 1, Time(45)));
        warmup.AddExercise(new WorkoutExercise(2, ex["Mountain Climbers"].Id, 1, Reps(20)));
        workout.AddSection(warmup);

        var main = new WorkoutSection(2, SectionType.Main, "Main Workout");
        main.AddExercise(new WorkoutExercise(1, ex["Plank"].Id, 3, Time(60)));
        main.AddExercise(new WorkoutExercise(2, ex["Bicycle Crunches"].Id, 4, Reps(25)));
        main.AddExercise(new WorkoutExercise(3, ex["Leg Raises"].Id, 3, Reps(15)));
        main.AddExercise(new WorkoutExercise(4, ex["Russian Twists"].Id, 3, Reps(30), notes: "15 each side"));
        main.AddExercise(new WorkoutExercise(5, ex["Side Plank"].Id, 3, Time(30), notes: "Each side"));
        workout.AddSection(main);

        var cooldown = new WorkoutSection(3, SectionType.CoolDown, "Cool Down");
        cooldown.AddExercise(new WorkoutExercise(1, ex["Superman"].Id, 2, Reps(12)));
        workout.AddSection(cooldown);

        return workout;
    }

    private static Workout BuildHIITDay(int day, string name, string description, Dictionary<string, Exercise> ex)
    {
        var workout = new Workout(day, name, description);
        
        var warmup = new WorkoutSection(1, SectionType.WarmUp, "Warm Up");
        warmup.AddExercise(new WorkoutExercise(1, ex["Jumping Jacks"].Id, 1, Reps(30)));
        warmup.AddExercise(new WorkoutExercise(2, ex["High Knees"].Id, 1, Time(30)));
        workout.AddSection(warmup);

        var main = new WorkoutSection(2, SectionType.Main, "Main Workout");
        main.AddExercise(new WorkoutExercise(1, ex["Burpees"].Id, 4, Reps(12), "A"));
        main.AddExercise(new WorkoutExercise(2, ex["Jump Squats"].Id, 4, Reps(15), "A"));
        main.AddExercise(new WorkoutExercise(3, ex["Mountain Climbers"].Id, 4, Reps(25), "A"));
        main.AddExercise(new WorkoutExercise(4, ex["Push-ups"].Id, 3, Reps(15)));
        main.AddExercise(new WorkoutExercise(5, ex["Plank"].Id, 3, Time(45)));
        workout.AddSection(main);

        var cooldown = new WorkoutSection(3, SectionType.CoolDown, "Cool Down");
        cooldown.AddExercise(new WorkoutExercise(1, ex["Glute Bridges"].Id, 2, Reps(12)));
        workout.AddSection(cooldown);

        return workout;
    }

    private static Workout BuildPushCoreDay(int day, string name, string description, Dictionary<string, Exercise> ex)
    {
        var workout = new Workout(day, name, description);
        
        var warmup = new WorkoutSection(1, SectionType.WarmUp, "Warm Up");
        warmup.AddExercise(new WorkoutExercise(1, ex["Jumping Jacks"].Id, 1, Reps(25)));
        workout.AddSection(warmup);

        var main = new WorkoutSection(2, SectionType.Main, "Main Workout");
        main.AddExercise(new WorkoutExercise(1, ex["Push-ups"].Id, 4, Reps(12)));
        main.AddExercise(new WorkoutExercise(2, ex["Diamond Push-ups"].Id, 3, Reps(8)));
        main.AddExercise(new WorkoutExercise(3, ex["Tricep Dips"].Id, 3, Reps(15)));
        main.AddExercise(new WorkoutExercise(4, ex["Plank"].Id, 3, Time(50)));
        main.AddExercise(new WorkoutExercise(5, ex["Bicycle Crunches"].Id, 3, Reps(20)));
        workout.AddSection(main);

        var cooldown = new WorkoutSection(3, SectionType.CoolDown, "Cool Down");
        cooldown.AddExercise(new WorkoutExercise(1, ex["Superman"].Id, 2, Reps(10)));
        workout.AddSection(cooldown);

        return workout;
    }

    private static Workout BuildRecoveryDay(int day, string name, string description, Dictionary<string, Exercise> ex)
    {
        var workout = new Workout(day, name, description);
        
        var warmup = new WorkoutSection(1, SectionType.WarmUp, "Warm Up");
        warmup.AddExercise(new WorkoutExercise(1, ex["Jumping Jacks"].Id, 1, Reps(15), notes: "Easy pace"));
        workout.AddSection(warmup);

        var main = new WorkoutSection(2, SectionType.Main, "Main Workout");
        main.AddExercise(new WorkoutExercise(1, ex["Squats"].Id, 2, Reps(10), notes: "Slow tempo"));
        main.AddExercise(new WorkoutExercise(2, ex["Glute Bridges"].Id, 2, Reps(12), notes: "Hold at top"));
        main.AddExercise(new WorkoutExercise(3, ex["Plank"].Id, 2, Time(30)));
        main.AddExercise(new WorkoutExercise(4, ex["Superman"].Id, 2, Reps(10)));
        workout.AddSection(main);

        var cooldown = new WorkoutSection(3, SectionType.CoolDown, "Cool Down");
        cooldown.AddExercise(new WorkoutExercise(1, ex["Side Plank"].Id, 1, Time(20), notes: "Each side"));
        workout.AddSection(cooldown);

        return workout;
    }
}
