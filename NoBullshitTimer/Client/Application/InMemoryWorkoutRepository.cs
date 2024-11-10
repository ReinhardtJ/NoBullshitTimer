using NoBullshitTimer.Client.Domain;

namespace NoBullshitTimer.Client.Application;

public class InMemoryWorkoutRepository : IWorkoutRepository
{
    private readonly Dictionary<string, Workout> _savedWorkouts = new();

    public InMemoryWorkoutRepository()
    {
        var hiit = WorkoutPresets.HIITPreset();
        var tabata = WorkoutPresets.TabataPreset();
        var boxing = WorkoutPresets.BoxingPreset();
        _savedWorkouts.Add(hiit.Name, hiit);
        _savedWorkouts.Add(tabata.Name, tabata);
        _savedWorkouts.Add(boxing.Name, boxing);
    }

    public event Action OnRepositoryChanged = () => {};

    public void Add(Workout workout)
    {
        var result = _savedWorkouts.TryAdd(workout.Name, workout);
        if (!result)
            throw new AddingWorkoutException(
                $"Can't add workout '{workout.Name}' to the store because a " +
                $"workout with that name already exists"
            );
        OnRepositoryChanged.Invoke();
    }

    public Workout Get(string name)
    {
        var result = _savedWorkouts.TryGetValue(name, out var workout);
        if (!result || workout == null)
            throw new WorkoutNotFoundException($"Workout with name '{name}' not in store");
        return workout;
    }

    public IList<Workout> GetAllWorkouts()
    {
        return _savedWorkouts.Select(kv => kv.Value).ToList();
    }


    public void Delete(string name)
    {
        _savedWorkouts.Remove(name);
        OnRepositoryChanged.Invoke();
    }
}
