using System.Text.Json;
using NoBullshitTimer.Client.Domain;
using NoBullshitTimer.Client.Framework;
using NoBullshitTimer.Client.Repositories;

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
        var addSuccessful = workouts.TryAdd(workout.Id, workout);
        if (!addSuccessful)
            throw new AddingWorkoutException(
                $"Can't add workout '{workout.Name}' to the store because a " +
                $"workout with that name already exists"
            );
        await UpdateLocalStorage(workouts);
        await OnRepositoryChanged.Invoke();
    }

    public async Task<Workout> Get(Guid workoutId)
    {
        var workouts = await GetWorkouts();
        var getSuccessful = workouts.TryGetValue(workoutId, out Workout? result);
        if (!getSuccessful || result == null)
            throw new WorkoutNotFoundException($"Workout with ID '{workoutId}' not found");

        return result;
    }

    public async Task Delete(Guid workoutId)
    {
        var workouts = await GetWorkouts();
        var workoutRemoveResult = workouts.Remove(workoutId);
        if (workoutRemoveResult)
            await UpdateLocalStorage(workouts);
    }

    public async Task<IList<Workout>> GetAllWorkouts()
    {
        return (await GetWorkouts()).Values.ToList();
    }

    private async Task UpdateLocalStorage(Dictionary<Guid, Workout> workouts)
    {
        var serializedWorkouts = JsonSerializer.Serialize(workouts);
        await _localStorageService.SetItemAsync("workouts", serializedWorkouts);
        await OnRepositoryChanged.Invoke();
    }

    private async Task<Dictionary<Guid, Workout>> GetWorkouts()
    {
        var result = await _localStorageService.GetItemAsync<Dictionary<Guid, Workout>>("workouts");
        return result ?? new Dictionary<Guid, Workout>();
    }
}