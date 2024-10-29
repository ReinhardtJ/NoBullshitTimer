using NoBullshitTimer.Client.Domain;
using NoBullshitTimer.Client.Framework;
using NoBullshitTimer.Client.UI;
using NoBullshitTimer.Client.UI.Components.Application;

namespace NoBullshitTimer.Client.Application;

public class WorkoutMapper
{
    public Workout? ToDomain(WorkoutForm workoutForm)
    {
        return new Workout(
            TimeFormat.ParseTime(workoutForm.PrepareTime, 10),
            TimeFormat.ParseTime(workoutForm.ExerciseTime, 40),
            TimeFormat.ParseTime(workoutForm.RestTime, 20),
            TimeFormat.ParseTime(workoutForm.CooldownTime, 60),
            Utils.ParseInt(workoutForm.SetsPerExercise, 3),
            workoutForm.Exercises.Select(exerciseInput => exerciseInput.Name).ToList(),
            workoutForm.CircularSets
        );
    }

    public WorkoutForm ToForm(Workout workout)
    {
        return new WorkoutForm(
            TimeFormat.UnparseTime(workout.PrepareTime),
            TimeFormat.UnparseTime(workout.ExerciseTime),
            TimeFormat.UnparseTime(workout.RestTime),
            TimeFormat.UnparseTime(workout.CooldownTime),
            workout.SetsPerExercise.ToString(),
            workout.Exercises.Select(exercise => new ExerciseInput(exercise)).ToList(),
            workout.CircularSets
        );
    }
}