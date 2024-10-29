using NoBullshitTimer.Client.Domain;

namespace NoBullshitTimer.Client.Application;

public class InMemoryWorkoutRepository : IWorkoutRepository
{
    private readonly Dictionary<string, Workout> _savedWorkouts = new();

    public InMemoryWorkoutRepository()
    {
        _savedWorkouts.Add("HIIT Preset", WorkoutPresets.HIITPreset());
        _savedWorkouts.Add("Tabata Preset", WorkoutPresets.TabataPreset());
        _savedWorkouts.Add("Boxing Preset", WorkoutPresets.BoxingPreset());
    }

    public void Add(Workout workout, string name)
    {
        var result = _savedWorkouts.TryAdd(name, workout);
        if (!result)
            throw new AddingWorkoutException(
                $"Can't add workout \"{name}\" to the store because a workout with that name already exists"
            );
    }

    public Workout Get(string name)
    {
        var result = _savedWorkouts.TryGetValue(name, out var workout);
        if (!result || workout == null)
            throw new WorkoutNotFoundException($"Workout with name \"{name}\" not in store");
        return workout;

    }

    public IEnumerable<KeyValuePair<string, Workout>> GetAllWorkouts()
    {
        return _savedWorkouts.AsEnumerable();
    }
}
