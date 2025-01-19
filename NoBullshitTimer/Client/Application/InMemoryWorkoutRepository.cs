using NoBullshitTimer.Client.Domain;

namespace NoBullshitTimer.Client.Application;

public class InMemoryWorkoutRepository : IWorkoutRepository
{
    private readonly IDictionary<string, Workout> _savedWorkouts;

    public InMemoryWorkoutRepository(IDictionary<string, Workout>? initialState = null)
    {
        _savedWorkouts = initialState ?? WorkoutPresets.DefaultPresets();
    }

    public event Func<Task> OnRepositoryChanged = () => Task.CompletedTask;

    public async Task Add(Workout workout)
    {
        var result = _savedWorkouts.TryAdd(workout.Name, workout);
        if (!result)
            throw new AddingWorkoutException(
                $"Can't add workout '{workout.Name}' to the store because a " +
                $"workout with that name already exists"
            );
        await OnRepositoryChanged.Invoke();
    }

    public Task<Workout> Get(string name)
    {
        var result = _savedWorkouts.TryGetValue(name, out var workout);
        if (!result || workout == null)
            throw new WorkoutNotFoundException($"Workout with name '{name}' not in store");
        return Task.FromResult(workout);
    }

    public Task<IList<Workout>> GetAllWorkouts()
    {
        return Task.FromResult<IList<Workout>>(_savedWorkouts.Select(kv => kv.Value).ToList());
    }


    public Task Delete(string name)
    {
        _savedWorkouts.Remove(name);
        OnRepositoryChanged.Invoke();
        return Task.CompletedTask;
    }
}
