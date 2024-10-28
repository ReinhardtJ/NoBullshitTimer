namespace NoBullshitTimer.Client.Domain;

public class Workout
{
    public TimeSpan PrepareTime;
    public TimeSpan ExerciseTime;
    public TimeSpan RestTime;
    public TimeSpan CooldownTime;
    public int SetsPerExercise;
    public List<string> Exercises;

    public Workout(
        TimeSpan prepareTime,
        TimeSpan exerciseTime,
        TimeSpan restTime,
        TimeSpan cooldownTime,
        int setsPerExercise,
        List<string> exercises
    )
    {
        PrepareTime = prepareTime;
        ExerciseTime = exerciseTime;
        RestTime = restTime;
        CooldownTime = cooldownTime;
        SetsPerExercise = setsPerExercise;
        Exercises = exercises;
    }

    public static Workout GetDefaultWorkout()
    {
        return new Workout(
            TimeSpan.FromSeconds(10),
            TimeSpan.FromSeconds(40),
            TimeSpan.FromSeconds(20),
            TimeSpan.FromSeconds(60),
            3,
            new () { "Exercise 1" }
        );
    }
}

