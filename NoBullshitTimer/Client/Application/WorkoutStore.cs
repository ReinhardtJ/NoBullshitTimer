using NoBullshitTimer.Client.Domain;
using NoBullshitTimer.Client.Framework;

namespace NoBullshitTimer.Client.Application;

public class WorkoutStore : IWorkoutStore
{
    private readonly IWorkoutRepository _workoutRepository;
    private Workout _selectedWorkout = null!;
    public event Action OnWorkoutStoreStateChanged = () => { };

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
            OnWorkoutStoreStateChanged.Invoke();
        }
    }

    private void LoadWorkoutsFromFromRepository()
    {
        _allWorkouts = _workoutRepository.GetAllWorkouts();
    }

    public IList<Workout> AllWorkouts => _allWorkouts;

    public void MoveWorkoutUp(Workout workout)
    {
        AllWorkouts.SwapToFront(workout);
        OnWorkoutStoreStateChanged.Invoke();
    }

    public void MoveWorkoutDown(Workout workout)
    {
        AllWorkouts.SwapToBack(workout);
        OnWorkoutStoreStateChanged.Invoke();
    }
}