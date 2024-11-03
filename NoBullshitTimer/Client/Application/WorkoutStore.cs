using NoBullshitTimer.Client.Domain;

namespace NoBullshitTimer.Client.Application;

public class WorkoutStore : IWorkoutStore
{
    private Workout _selectedWorkout;
    public event Action<Workout> OnWorkoutChanged = _ => { };

    private IList<Workout> _allWorkouts;


    public WorkoutStore(IWorkoutRepository workoutRepository)
    {
        try
        {
            _allWorkouts = workoutRepository.GetAllWorkouts();
            SelectedWorkout = workoutRepository.GetAllWorkouts().First();
        }
        catch (InvalidOperationException)
        {
            SelectedWorkout = WorkoutPresets.HIITPreset();
        }
    }

    public Workout SelectedWorkout
    {
        get => _selectedWorkout;
        set
        {
            _selectedWorkout = value;
            OnWorkoutChanged.Invoke(value);
        }
    }

    public IList<Workout> AllWorkouts => _allWorkouts;
}