using NoBullshitTimer.Client.Domain;

namespace NoBullshitTimer.Client.Application;

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
    event Action OnRepositoryChanged;

    /// <summary>
    /// Adds a workout to this store under a specific name. There can't be more than one
    /// workout with the same name in a store
    /// </summary>
    /// <param name="workout"></param>
    /// <param name="name"></param>
    /// <exception cref="AddingWorkoutException">
    /// When attempting to add a workout with a name that already exists
    /// in this store
    /// </exception>
    void Add(Workout workout);

    /// <summary>
    /// Retrieves a workout from this repository using its name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="WorkoutNotFoundException">
    /// When attempting to retrieve a workout using a name that doesn't exist
    /// in this store
    /// </exception>
    Workout Get(string name);

    /// <summary>
    /// Deletes a workout from this repository.
    /// </summary>
    /// <param name="name"></param>
    void Delete(string name);


    IList<Workout> GetAllWorkouts();
}
