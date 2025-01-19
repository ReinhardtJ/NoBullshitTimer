using NoBullshitTimer.Client.Domain;
using NoBullshitTimer.Client.Framework;

namespace NoBullshitTimer.Client.Application;

public class WorkoutStore : IWorkoutStore
{
    private readonly IWorkoutRepository _workoutRepository;
    private Workout _selectedWorkout = null!;

    public event Action OnWorkoutStoreStateChanged = () => { };

    private IList<Workout> _allWorkouts = new List<Workout>();


    private WorkoutStore(IWorkoutRepository workoutRepository)
    {
        _workoutRepository = workoutRepository;
    }

    public static async Task<WorkoutStore> Create(IWorkoutRepository workoutRepository)
    {
        var self = new WorkoutStore(workoutRepository);
        try
        {
            await self.LoadWorkoutsFromFromRepository();
            self.SelectedWorkout = (await workoutRepository.GetAllWorkouts()).First();
        }
        catch (InvalidOperationException)
        {
            self.SelectedWorkout = WorkoutPresets.HIITPreset();
        }

        self._workoutRepository.OnRepositoryChanged += self.LoadWorkoutsFromFromRepository;
        return self;
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