using System.Text.Json;
using NoBullshitTimer.Client.Domain;
using NoBullshitTimer.Client.Framework;

namespace NoBullshitTimer.Client.Application;

public class LocalStorageWorkoutRepository : IWorkoutRepository
{
    private readonly ILocalStorageService _localStorageService;

    public event Func<Task> OnRepositoryChanged = () => Task.CompletedTask;

    public LocalStorageWorkoutRepository(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public async Task Add(Workout workout)
    {
        var workouts = await GetWorkouts();
        var addSuccessful = workouts.TryAdd(workout.Name, workout);
        if (!addSuccessful)
            throw new AddingWorkoutException(
                $"Can't add workout '{workout.Name}' to the store because a " +
                $"workout with that name already exists"
            );
        await UpdateLocalStorage(workouts);
        await OnRepositoryChanged.Invoke();
    }

    public async Task<Workout> Get(string name)
    {
        var workouts = await GetWorkouts();
        var getSuccessful = workouts.TryGetValue(name, out Workout? result);
        if (!getSuccessful || result == null)
            throw new WorkoutNotFoundException($"Workout with name '{name}' not found");

        return result;
    }

    public async Task Delete(string name)
    {
        var workouts = await GetWorkouts();
        workouts.Remove(name);
        await UpdateLocalStorage(workouts);
    }

    public async Task<IList<Workout>> GetAllWorkouts()
    {
        return (await GetWorkouts()).Values.ToList();
    }

    private async Task UpdateLocalStorage(Dictionary<string, Workout> workouts)
    {
        var serializedWorkouts = JsonSerializer.Serialize(workouts);
        await _localStorageService.SetItemAsync("workouts", serializedWorkouts);
        await OnRepositoryChanged.Invoke();
    }

    private async Task<Dictionary<string, Workout>> GetWorkouts()
    {
        var result = await _localStorageService.GetItemAsync<Dictionary<string, Workout>>("workouts");
        return result ?? new Dictionary<string, Workout>();
    }
}