namespace NoBullshitTimer.Client.UI.Components.Application;

public class WorkoutForm
{
    private string _prepareTime;
    private string _exerciseTime;
    private string _restTime;
    private string _cooldownTime;
    private string _setsPerExercise;

    public string PrepareTime
    {
        get => _prepareTime;
        set
        {
            _prepareTime = value;
            Dirty = true;
        }
    }

    public string ExerciseTime
    {
        get => _exerciseTime;
        set
        {
            _exerciseTime = value;
            Dirty = true;
        }
    }

    public string RestTime
    {
        get => _restTime;
        set
        {
            _restTime = value;
            Dirty = true;
        }
    }

    public string CooldownTime
    {
        get => _cooldownTime;
        set
        {
            _cooldownTime = value;
            Dirty = true;
        }
    }

    public string SetsPerExercise
    {
        get => _setsPerExercise;
        set
        {
            _setsPerExercise = value;
            Dirty = true;
        }
    }

    public List<ExerciseInput> Exercises { get; }
    public bool Dirty { get; set; }

    public WorkoutForm(
        string prepareTime,
        string exerciseTime,
        string restTime,
        string cooldownTime,
        string setsPerExercise,
        List<ExerciseInput> exercises
    ) {
        _prepareTime = prepareTime;
        _exerciseTime = exerciseTime;
        _restTime = restTime;
        _cooldownTime = cooldownTime;
        _setsPerExercise = setsPerExercise;
        Exercises = exercises;
        Dirty = false;
    }

    public void DeleteExercise(ExerciseInput exerciseInput)
    {
        Exercises.Remove(exerciseInput);
        Dirty = true;
    }

    public void AddExercise(ExerciseInput exerciseInput)
    {
        var insertIndex = Exercises.IndexOf(exerciseInput) + 1;
        Exercises.Insert(insertIndex, new ExerciseInput ($"Exercise {insertIndex + 1}"));
        Dirty = true;
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