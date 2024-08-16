using NoBullshitTimer.Client.Domain;
using OneOf.Monads;

namespace NoBullshitTimer.Client.Application;

public class InMemoryWorkoutPlanStore : IWorkoutPlanStore
{
    private Dictionary<string, WorkoutPlan> _workoutPlans = new();

    public bool Add(WorkoutPlan workoutPlan, string name)
    {
        return _workoutPlans.TryAdd(name, workoutPlan);
    }

    public Option<WorkoutPlan> GetByName(string name)
    {
        if (_workoutPlans.TryGetValue(name, out var result))
            return result;

        return new None();
    }

    public IEnumerable<KeyValuePair<string, WorkoutPlan>> GetAllWorkoutPlans()
    {
        return _workoutPlans.AsEnumerable();
    }
}
