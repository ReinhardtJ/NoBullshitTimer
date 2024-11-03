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
    /// Adds a workout to this store under a specific name. There can't be more than one
    /// workout with the same name in a store
    /// </summary>
    /// <param name="workout"></param>
    /// <param name="name"></param>
    /// <exception cref="AddingWorkoutException">
    /// Gets thrown when attempting to add a workout with a name that already exists
    /// in this store
    /// </exception>
    void Add(Workout workout);

    /// <summary>
    /// Retrieves a workout from this store using its name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="WorkoutNotFoundException">
    /// Gets thrown when attempting to retrieve a workout using a name that doesn't exists
    /// in this store
    /// </exception>
    Workout Get(string name);

    IList<Workout> GetAllWorkouts();
}
