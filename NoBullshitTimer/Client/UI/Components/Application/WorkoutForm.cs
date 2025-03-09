namespace NoBullshitTimer.Client.UI.Components.Application;

public class WorkoutForm
{
    private Guid _id;
    private string _name;
    private string _prepareTime;
    private string _exerciseTime;
    private string _restTime;
    private string _cooldownTime;
    private string _setsPerExercise;
    private bool _circularSets;
    private readonly List<ExerciseInput> _exercises;
    private bool _dirty;

    public event Action OnFormChanged = () => { };

    public Guid WorkoutId => _id;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            Dirty = true;
            OnFormChanged.Invoke();
        }
    }

    public string PrepareTime
    {
        get => _prepareTime;
        set
        {
            _prepareTime = value;
            Dirty = true;
            OnFormChanged.Invoke();
        }
    }

    public string ExerciseTime
    {
        get => _exerciseTime;
        set
        {
            _exerciseTime = value;
            Dirty = true;
            OnFormChanged.Invoke();
        }
    }

    public string RestTime
    {
        get => _restTime;
        set
        {
            _restTime = value;
            Dirty = true;
            OnFormChanged.Invoke();
        }
    }

    public string CooldownTime
    {
        get => _cooldownTime;
        set
        {
            _cooldownTime = value;
            Dirty = true;
            OnFormChanged.Invoke();
        }
    }

    public string SetsPerExercise
    {
        get => _setsPerExercise;
        set
        {
            _setsPerExercise = value;
            Dirty = true;
            OnFormChanged.Invoke();
        }
    }

    public bool CircularSets
    {
        get => _circularSets;
        set
        {
            _circularSets = value;
            Dirty = true;
            OnFormChanged.Invoke();
        }
    }

    public ICollection<ExerciseInput> Exercises => _exercises;

    public bool Dirty
    {
        get => _dirty;
        set
        {
            _dirty = value;
            OnFormChanged.Invoke();
        }
    }

    public WorkoutForm(
        Guid id,
        string name,
        string prepareTime,
        string exerciseTime,
        string restTime,
        string cooldownTime,
        string setsPerExercise,
        List<ExerciseInput> exercises,
        bool circularSets
    )
    {
        _id = id;
        _name = name;
        _prepareTime = prepareTime;
        _exerciseTime = exerciseTime;
        _restTime = restTime;
        _cooldownTime = cooldownTime;
        _setsPerExercise = setsPerExercise;
        _exercises = exercises;
        _circularSets = circularSets;
        Dirty = false;
    }

    public void DeleteExercise(ExerciseInput exerciseInput)
    {
        _exercises.Remove(exerciseInput);
        Dirty = true;
        OnFormChanged.Invoke();
    }

    public void AddExercise(ExerciseInput addAfter)
    {
        var insertIndex = _exercises.IndexOf(addAfter) + 1;
        _exercises.Insert(insertIndex, new ExerciseInput ($"Exercise {insertIndex + 1}"));
        Dirty = true;
        OnFormChanged.Invoke();
    }

    public void MoveExerciseUp(ExerciseInput exerciseInput)
    {
        var index = _exercises.IndexOf(exerciseInput);
        if (index == 0)
            return;
        var previous = _exercises[index - 1];
        _exercises[index] = previous;
        _exercises[index - 1] = exerciseInput;
        Dirty = true;
        OnFormChanged.Invoke();
    }

    public void MoveExerciseDown(ExerciseInput exerciseInput)
    {
        var index = _exercises.IndexOf(exerciseInput);
        if (index == _exercises.Count - 1)
            return;
        var next = _exercises[index + 1];
        _exercises[index] = next;
        _exercises[index + 1] = exerciseInput;
        Dirty = true;
        OnFormChanged.Invoke();
    }

    public bool IsFirstExercise(ExerciseInput exerciseInput)
    {
        return exerciseInput == _exercises[0];
    }

    public bool IsLastExercise(ExerciseInput exerciseInput)
    {
        return exerciseInput == _exercises[^1];
    }
}