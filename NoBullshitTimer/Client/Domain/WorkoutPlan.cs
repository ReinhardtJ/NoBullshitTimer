namespace NoBullshitTimer.Client.Domain;

public record WorkoutPlan(
    TimeSpan PrepareTime,
    TimeSpan WorkTime,
    TimeSpan RestTime,
    TimeSpan CooldownTime,
    int SetsPerExercise,
    List<string> Exercises
);
