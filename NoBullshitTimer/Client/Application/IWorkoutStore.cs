using NoBullshitTimer.Client.Domain;

namespace NoBullshitTimer.Client.Application;

public interface IWorkoutStore
{
    public Workout SelectedWorkout { get; set; }
    public event Action<Workout> OnWorkoutChanged;
    public IList<Workout> AllWorkouts { get; }
}