using NoBullshitTimer.Client.Domain;

namespace NoBullshitTimer.Client.Application;

public class WorkoutState : IWorkoutState
{
    private Workout _workout;
    public event Action<Workout> OnWorkoutChanged = _ => { };



    public WorkoutState(IWorkoutRepository workoutRepository)
    {
        try
        {
            Workout = workoutRepository.GetAllWorkouts().First().Value;
        }
        catch (InvalidOperationException)
        {
            Workout = WorkoutPresets.HIITPreset();
        }
    }

    public Workout Workout
    {
        get => _workout;
        set
        {
            _workout = value;
            OnWorkoutChanged.Invoke(value);
        }
    }
}