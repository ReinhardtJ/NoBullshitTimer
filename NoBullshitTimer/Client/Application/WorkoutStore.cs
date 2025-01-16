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

    }

    public static async Task<WorkoutStore> Init(IWorkoutRepository workoutRepository)
    {
        var workoutStore = new WorkoutStore(workoutRepository);
        try
        {
            await workoutStore.LoadWorkoutsFromFromRepository();
            workoutStore.SelectedWorkout = (await workoutRepository.GetAllWorkouts()).First();
        }
        catch (InvalidOperationException)
        {
            workoutStore.SelectedWorkout = WorkoutPresets.HIITPreset();
        }

        workoutStore._workoutRepository.OnRepositoryChanged += workoutStore.LoadWorkoutsFromFromRepository;
        return workoutStore;
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

    private async Task LoadWorkoutsFromFromRepository()
    {
        _allWorkouts = await _workoutRepository.GetAllWorkouts();
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