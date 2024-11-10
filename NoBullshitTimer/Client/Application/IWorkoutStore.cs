using NoBullshitTimer.Client.Domain;

namespace NoBullshitTimer.Client.Application;

public interface IWorkoutStore
{
    public Workout SelectedWorkout { get; set; }
    public event Action OnWorkoutStoreStateChanged;
    public IList<Workout> AllWorkouts { get; }
    public void MoveWorkoutUp(Workout workout);
    public void MoveWorkoutDown(Workout workout);
}