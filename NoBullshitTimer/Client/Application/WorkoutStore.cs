using NoBullshitTimer.Client.Domain;

namespace NoBullshitTimer.Client.Application;

public class WorkoutStore : IWorkoutStore
{
    private readonly IWorkoutRepository _workoutRepository;
    private Workout _selectedWorkout = null!;
    public event Action<Workout> OnWorkoutChanged = _ => { };

    private IList<Workout> _allWorkouts = new List<Workout>();


    public WorkoutStore(IWorkoutRepository workoutRepository)
    {
        _workoutRepository = workoutRepository;
        try
        {
            LoadWorkoutsFromFromRepository();
            SelectedWorkout = workoutRepository.GetAllWorkouts().First();
        }
        catch (InvalidOperationException)
        {
            SelectedWorkout = WorkoutPresets.HIITPreset();
        }

        _workoutRepository.OnRepositoryChanged += LoadWorkoutsFromFromRepository;
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

    private void LoadWorkoutsFromFromRepository()
    {
        _allWorkouts = _workoutRepository.GetAllWorkouts();
    }

    public IList<Workout> AllWorkouts => _allWorkouts;
}