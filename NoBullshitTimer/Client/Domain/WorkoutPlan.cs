namespace NoBullshitTimer.Client.Domain;

public record WorkoutPlan(
    int prepareTime,
    int workTime,
    int restTime,
    int cooldownTime,
    int setsPerExercise,
    List<string> exercises
);
