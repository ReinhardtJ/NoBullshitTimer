using System.Text.Json;
using NoBullshitTimer.Client.Domain;
using NoBullshitTimer.Client.Framework;

namespace NoBullshitTimer.Client.Application;

public class LocalStorageWorkoutRepository : IWorkoutRepository
{
    private readonly LocalStorageService _localStorageService;

    public event Func<Task> OnRepositoryChanged = () => Task.CompletedTask;

    public LocalStorageWorkoutRepository(LocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public async Task Add(Workout workout)
    {
        var workouts = await GetWorkouts();
        workouts[workout.Name] = workout;
        await UpdateLocalStorage(workouts);
    }

    public async Task<Workout> Get(string name)
    {
        return (await GetWorkouts())[name];
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
        return await _localStorageService.GetItemAsync<Dictionary<string, Workout>>("workouts");
    }
}