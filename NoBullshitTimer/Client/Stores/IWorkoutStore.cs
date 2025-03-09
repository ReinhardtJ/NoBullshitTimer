using NoBullshitTimer.Client.Domain;

namespace NoBullshitTimer.Client.Stores;

public interface IWorkoutStore
{
    public Workout SelectedWorkout { get; set; }
    public event Action OnWorkoutStoreStateChanged;
    public IList<Workout> AllWorkouts { get; }
    public void MoveWorkoutUp(Workout workout);
    public void MoveWorkoutDown(Workout workout);
    public Task UpdateWorkout(Workout workout);
}