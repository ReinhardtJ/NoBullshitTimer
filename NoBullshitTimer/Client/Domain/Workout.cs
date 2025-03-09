using System.Text.Json;
using System.Text.Json.Serialization;
using NoBullshitTimer.Client.Framework;

namespace NoBullshitTimer.Client.Domain;


public class Workout : IEquatable<Workout>
{
    public Guid Id { get; init; }
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
        Id = Guid.NewGuid();
        Name = "";
        Exercises = new List<string>();
    }

    public Workout(
        Guid id,
        string name,
        TimeSpan prepareTime,
        TimeSpan exerciseTime,
        TimeSpan restTime,
        TimeSpan cooldownTime,
        int setsPerExercise,
        List<string> exercises,
        bool circularSets)
    {
        Id = id;
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
        return Id == other.Id
               && Name == other.Name
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
        var hash1 = HashCode.Combine(Id, Name, PrepareTime, ExerciseTime, RestTime);
        var hash2 = HashCode.Combine(CooldownTime, SetsPerExercise, Exercises, CircularSets);
        return HashCode.Combine(hash1, hash2);
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
}

public static class WorkoutPresets
{
    private static readonly Guid HIITPresetId = new("74a183a7-d77f-4906-9766-c191117d5651");
    private static readonly Guid TabataPresetId = new("0dfa3425-3a72-4163-af65-352b7abc1044");
    private static readonly Guid BoxingPresetId = new("27cd02dc-5835-4b28-8d6b-2aa562907765");

    public static Workout HIITPreset()
    {
        return new Workout(
            HIITPresetId,
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
            TabataPresetId,
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
            BoxingPresetId,
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

    public static Dictionary<Guid, Workout> DefaultPresets()
    {
        return new Dictionary<Guid, Workout>
        {
            { HIITPresetId, HIITPreset() },
            { TabataPresetId, TabataPreset() },
            { BoxingPresetId, BoxingPreset() }
        };
    }
}
