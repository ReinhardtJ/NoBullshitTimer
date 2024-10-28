namespace NoBullshitTimer.Client.UI.Components.Application;

public class WorkoutForm
{
    public string PrepareTime { get; set; }
    public string ExerciseTime { get; set; }
    public string RestTime { get; set; }
    public string CooldownTime { get; set; }
    public string SetsPerExercise { get; set; }
    public List<ExerciseInput> Exercises { get; set; }

    public WorkoutForm(
        string prepareTime,
        string exerciseTime,
        string restTime,
        string cooldownTime,
        string setsPerExercise,
        List<ExerciseInput> exercises
    ) {
        PrepareTime = prepareTime;
        ExerciseTime = exerciseTime;
        RestTime = restTime;
        CooldownTime = cooldownTime;
        SetsPerExercise = setsPerExercise;
        Exercises = exercises;
    }

    public static WorkoutForm GetDefaultWorkoutForm()
    {
        return new WorkoutForm(
            "00:10",
            "00:40",
            "00:20",
            "01:00",
            "3",
            new(){ new ExerciseInput("Exercise 1") }
        );

    }
}