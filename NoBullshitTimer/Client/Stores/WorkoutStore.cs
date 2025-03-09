using NoBullshitTimer.Client.Domain;
using NoBullshitTimer.Client.Framework;
using NoBullshitTimer.Client.Repositories;

namespace NoBullshitTimer.Client.Stores;

public class WorkoutStore : IWorkoutStore
{
    private readonly IWorkoutRepository _workoutRepository;
    private int _selectedWorkoutIndex = -1;

    public IList<Workout> AllWorkouts { get; private set; } = new List<Workout>();

    public event Action OnWorkoutStoreStateChanged = () => { };



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
        get => AllWorkouts[_selectedWorkoutIndex];
        set
        {
            var index = AllWorkouts.IndexOf(value);
            if (index == -1)
            {
                throw new Exception(
                    "The workout you tried to set as the current workout dosen't exist in the current " +
                    "workout list");
            }
            _selectedWorkoutIndex = index;
            OnWorkoutStoreStateChanged.Invoke();
        }
    }

    private async Task LoadWorkoutsFromFromRepository()
    {
        AllWorkouts = await _workoutRepository.GetAllWorkouts();
    }

    public async Task UpdateWorkout(Workout workout)
    {
        await _workoutRepository.Delete(workout.Id);
        await _workoutRepository.Add(workout);
        OnWorkoutStoreStateChanged.Invoke();
    }


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