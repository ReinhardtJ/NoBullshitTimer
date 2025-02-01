using System.Text.Json.Serialization;
using NoBullshitTimer.Client.Framework;

namespace NoBullshitTimer.Client.Domain;

public class Workout : IEquatable<Workout>
{
    public string Name { get; init; }

    [JsonConverter(typeof(JsonTimeSpanConverter))]
    public TimeSpan PrepareTime { get; init; }
    [JsonConverter(typeof(JsonTimeSpanConverter))]
    public TimeSpan ExerciseTime { get; init; }
    [JsonConverter(typeof(JsonTimeSpanConverter))]
    public TimeSpan RestTime { get; init; }
    [JsonConverter(typeof(JsonTimeSpanConverter))]
    public TimeSpan CooldownTime { get; init; }

    public int SetsPerExercise { get; init; }
    public List<string> Exercises { get; init; }
    public bool CircularSets { get; init; }

    // parameterless ctor for json deserialization
    public Workout()
    {
        Name = "";
        Exercises = new List<string>();
    }

    public Workout(
        string name,
        TimeSpan prepareTime,
        TimeSpan exerciseTime,
        TimeSpan restTime,
        TimeSpan cooldownTime,
        int setsPerExercise,
        List<string> exercises,
        bool circularSets)
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

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != GetType())
            return false;
        return Equals((Workout)obj);
    }

    public bool Equals(Workout? other)
    {
        if (other is null)
            return false;
        return Name == other.Name
               && PrepareTime.Equals(other.PrepareTime)
               && ExerciseTime.Equals(other.ExerciseTime)
               && RestTime.Equals(other.RestTime)
               && CooldownTime.Equals(other.CooldownTime)
               && SetsPerExercise == other.SetsPerExercise
               && Exercises.SequenceEqual(other.Exercises)
               && CircularSets == other.CircularSets;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Name, PrepareTime, ExerciseTime, RestTime, CooldownTime, SetsPerExercise, Exercises, CircularSets
        );
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
