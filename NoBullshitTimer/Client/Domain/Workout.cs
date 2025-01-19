namespace NoBullshitTimer.Client.Domain;

public record Workout(
    string Name,
    TimeSpan PrepareTime,
    TimeSpan ExerciseTime,
    TimeSpan RestTime,
    TimeSpan CooldownTime,
    int SetsPerExercise,
    List<string> Exercises,
    bool CircularSets)
{
    public bool IsLastExercise(string exercise)
    {
        return exercise == Exercises[^1];
    }

    public bool IsLastSet(int set)
    {
        return set == SetsPerExercise;
    }

    public virtual bool Equals(Workout? other)
    {
        if (other is null) return false;
        return Name == other.Name &&
               PrepareTime == other.PrepareTime &&
               ExerciseTime == other.ExerciseTime &&
               RestTime == other.RestTime &&
               CooldownTime == other.CooldownTime &&
               SetsPerExercise == other.SetsPerExercise &&
               CircularSets == other.CircularSets &&
               Exercises.SequenceEqual(other.Exercises);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Name);
        hash.Add(PrepareTime);
        hash.Add(ExerciseTime);
        hash.Add(RestTime);
        hash.Add(CooldownTime);
        hash.Add(SetsPerExercise);
        hash.Add(CircularSets);
        foreach (var exercise in Exercises)
        {
            hash.Add(exercise);
        }
        return hash.ToHashCode();
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

    public static Dictionary<string, Workout> DefaultPresets()
    {
        return new Dictionary<string, Workout>
        {
            { HIITPreset().Name, HIITPreset() },
            { TabataPreset().Name, TabataPreset() },
            { BoxingPreset().Name, BoxingPreset() }
        };
    }
}
