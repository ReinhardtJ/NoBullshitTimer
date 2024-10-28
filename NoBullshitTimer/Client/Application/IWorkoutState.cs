using NoBullshitTimer.Client.Domain;

namespace NoBullshitTimer.Client.Application;

public interface IWorkoutState
{
    public Workout Workout { get; set; }
    public event Action<Workout> OnWorkoutChanged;
}