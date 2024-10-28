using NoBullshitTimer.Client.Domain;

namespace NoBullshitTimer.Client.Application;

public class WorkoutState : IWorkoutState
{
    private Workout? _workout;
    public event Action<Workout?> OnWorkoutChanged = _ => { };

    public WorkoutState()
    {
        _workout = Workout.GetDefaultWorkout();
    }

    public Workout? Workout
    {
        get => _workout;
        set
        {
            _workout = value;
            OnWorkoutChanged.Invoke(value);
        }
    }
}