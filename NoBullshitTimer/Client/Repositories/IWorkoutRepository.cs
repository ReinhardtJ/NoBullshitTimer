using NoBullshitTimer.Client.Domain;

namespace NoBullshitTimer.Client.Repositories;

public class AddingWorkoutException : Exception
{
    public AddingWorkoutException(string? message) : base(message)
    {
    }
}

public class WorkoutNotFoundException : Exception
{
    public WorkoutNotFoundException(string? message) : base(message)
    {
    }
}

public interface IWorkoutRepository
{
    /// <summary>
    /// Invoked whenever the repository changes due to a workout being added,
    /// updated or deleted
    /// </summary>
    event Func<Task> OnRepositoryChanged;

    /// <summary>
    /// Adds a workout to this store under a specific ID. There can't be more
    /// than one workout with the same ID in a store. If the workout has been
    /// successfully added, a OnRepositoryChanged event is invoked.
    /// </summary>
    /// <exception cref="AddingWorkoutException">
    /// When attempting to add a workout with an ID that already exists
    /// in this store
    /// </exception>
    Task Add(Workout workout);

    /// <summary>
    /// Retrieves a workout from this repository using its name.
    /// </summary>
    /// <exception cref="WorkoutNotFoundException">
    /// When attempting to retrieve a workout using a name that doesn't exist
    /// in this store
    /// </exception>
    Task<Workout> Get(Guid id);

    /// <summary>
    /// Deletes a workout from this repository. Invokes an OnRepositoryChanged
    /// event if the workout with the given ID has successfully been deleted
    /// </summary>
    Task Delete(Guid id);

    Task<IList<Workout>> GetAllWorkouts();
}
