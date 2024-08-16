using NoBullshitTimer.Client.Domain;
using OneOf.Monads;

namespace NoBullshitTimer.Client.Application;

public interface IWorkoutPlanStore
{
    // todo: better return type (bool not expressive)
    bool Add(WorkoutPlan workoutPlan, string name);
    // todo: better return type (what happens when workout with that name doesn't exist?)
    Option<WorkoutPlan> GetByName(string name);

    IEnumerable<KeyValuePair<string, WorkoutPlan>> GetAllWorkoutPlans();
}
