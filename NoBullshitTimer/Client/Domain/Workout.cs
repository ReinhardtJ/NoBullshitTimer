namespace NoBullshitTimer.Client.Domain;

public class Workout
{
    public string Name;
    public TimeSpan PrepareTime;
    public TimeSpan ExerciseTime;
    public TimeSpan RestTime;
    public TimeSpan CooldownTime;
    public int SetsPerExercise;
    public List<string> Exercises;
    public bool CircularSets;

    public Workout(
        string name,
        TimeSpan prepareTime,
        TimeSpan exerciseTime,
        TimeSpan restTime,
        TimeSpan cooldownTime,
        int setsPerExercise,
        List<string> exercises,
        bool circularSets
    )
    {
        Name = name;
        PrepareTime = prepareTime;
        ExerciseTime = exerciseTime;
        RestTime = restTime;
        CooldownTime = cooldownTime;
        SetsPerExercise = setsPerExercise;
        Exercises = exercises;
        CircularSets = circularSets;
    }

    public bool IsLastExercise(string exercise)
    {
        return exercise == Exercises[^1];
    }

    public bool IsLastSet(int set)
    {
        return set == SetsPerExercise;
    }
}

public static class WorkoutPresets
{
    public static Workout HIITPreset()
    {
        return new Workout(
            "HIIT",
            TimeSpan.FromMinutes(1),
            TimeSpan.FromSeconds(40),
            TimeSpan.FromSeconds(20),
            TimeSpan.FromMinutes(1),
            3,
            new () { "Exercise 1" },
            false
        );

    }

    public static Workout TabataPreset()
    {
        return new Workout(
            "Tabata",
            TimeSpan.FromMinutes(1),
            TimeSpan.FromSeconds(20),
            TimeSpan.FromSeconds(10),
            TimeSpan.FromMinutes(1),
            8,
            new () { "Work" },
            false
        );
    }

    public static Workout BoxingPreset()
    {
        return new Workout(
            "Boxing",
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(3),
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(1),
            12,
            new () { "Round" },
            false
        );
    }
}

