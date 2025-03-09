using NoBullshitTimer.Client.Domain;

namespace NoBullshitTimer.Client.Repositories;

public class InMemoryWorkoutRepository : IWorkoutRepository
{
    private readonly IDictionary<Guid, Workout> _savedWorkouts;

    public InMemoryWorkoutRepository(IDictionary<Guid, Workout>? initialState = null)
    {
        _savedWorkouts = initialState ?? WorkoutPresets.DefaultPresets();
    }

    public InMemoryWorkoutRepository(IList<Workout> initialWorkouts)
    {
        _savedWorkouts = initialWorkouts.ToDictionary(workout => workout.Id, workout => workout);
    }

    public event Func<Task> OnRepositoryChanged = () => Task.CompletedTask;

    public async Task Add(Workout workout)
    {
        var result = _savedWorkouts.TryAdd(workout.Id, workout);
        if (!result)
            throw new AddingWorkoutException(
                $"Can't add workout '{workout.Name}' to the store because a " +
                $"workout with that id already exists"
            );
        await OnRepositoryChanged.Invoke();
    }

    public Task<Workout> Get(Guid id)
    {
        var result = _savedWorkouts.TryGetValue(id, out var workout);
        if (!result || workout == null)
            throw new WorkoutNotFoundException($"Workout with ID '{id}' not in store");
        return Task.FromResult(workout);
    }

    public Task<IList<Workout>> GetAllWorkouts()
    {
        return Task.FromResult<IList<Workout>>(_savedWorkouts.Select(kv => kv.Value).ToList());
    }


    public Task Delete(Guid id)
    {
        var removeResult = _savedWorkouts.Remove(id);
        if (removeResult)
            OnRepositoryChanged.Invoke();
        return Task.CompletedTask;
    }
}
