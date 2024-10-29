using NoBullshitTimer.Client.Application;
using NoBullshitTimer.Client.Framework;

namespace NoBullshitTimer.Client.Domain;

public class IntervalTimer : IDisposable, IAsyncDisposable
{
    private Timer? _timer;
    private LinkedListNode<Interval>? CurrentIntervalNode { get; set; }

    public event Action OnTimerStateChanged = () => { };

    public int CurrentIntervalNr { get; private set; }
    public int SecondsLeft { get; private set; }
    public bool TimerPaused = true;
    private LinkedList<Interval> _intervals = new();

    public IntervalTimer(IWorkoutState workoutState)
    {
        workoutState.OnWorkoutChanged += InitIntervalTimer;
        InitIntervalTimer(workoutState.Workout);
    }

    private void InitIntervalTimer(Workout workout)
    {
        _intervals = new();
        _intervals.AddLast(new Ready());
        _intervals.AddLast(new Prepare(workout.PrepareTime.TotalSecondsInt()));
        if (workout.CircularSets)
            InitIntervalsCircular(workout);
        else
            InitIntervalsStraight(workout);

        Console.WriteLine(workout.CooldownTime);
        Console.WriteLine(workout.CooldownTime.TotalSecondsInt());
        _intervals.AddLast(new Cooldown(workout.CooldownTime.TotalSecondsInt()));
        _intervals.AddLast(new Done());
        CurrentIntervalNode = _intervals.First!;
        CurrentIntervalNr = 1;
        SecondsLeft = CurrentIntervalNode.Value.IntervalLength;

        if (_timer != null)
            _timer.Dispose();

        _timer = new Timer(_ =>
        {
            if (TimerPaused)
                return;
            Tick();
            OnTimerStateChanged.Invoke();
        }, null, 0L, 1000L);
        OnTimerStateChanged.Invoke();
    }

    private void InitIntervalsStraight(Workout workout)
    {
        foreach (var exercise in workout.Exercises)
        {
            for (var i = 1; i <= workout.SetsPerExercise; i++)
            {
                _intervals.AddLast(new Work(workout.ExerciseTime.TotalSecondsInt(), exercise));
                if (workout.IsLastSet(i) && workout.IsLastExercise(exercise))
                    break;

                _intervals.AddLast(new Rest(workout.RestTime.TotalSecondsInt()));
            }
        }
    }

    private void InitIntervalsCircular(Workout workout)
    {
        for (var i = 1; i <= workout.SetsPerExercise; i++)
        {
            foreach (var exercise in workout.Exercises)
            {
                _intervals.AddLast(new Work(workout.ExerciseTime.TotalSecondsInt(), exercise));
                if (workout.IsLastSet(i) && workout.IsLastExercise(exercise))
                    break;

                _intervals.AddLast(new Rest(workout.RestTime.TotalSecondsInt()));
            }
        }
    }

    public Interval? CurrentInterval => CurrentIntervalNode?.Value;
    public Interval? NextInterval => CurrentIntervalNode?.Next?.Value;

    public void Tick()
    {
        SecondsLeft -= 1;

        if (SecondsLeft <= 0)
            GoToNextInterval();
    }

    public void GoToPreviousInterval()
    {
        if (CurrentIntervalNode?.Previous is null)
            return;

        if (CurrentIntervalNode.Value.SecondsIntoInterval(SecondsLeft) <= 5)
        {
            CurrentIntervalNode = CurrentIntervalNode.Previous;
            CurrentIntervalNr -= 1;
        }

        SecondsLeft = CurrentIntervalNode.Value.IntervalLength;
        if (CurrentInterval is Ready)
            TimerPaused = true;

        OnTimerStateChanged.Invoke();
    }

    public void GoToNextInterval()
    {
        if (CurrentIntervalNode?.Next is null)
            return;

        if (NextInterval is Done)
            TimerPaused = true;

        CurrentIntervalNode = CurrentIntervalNode.Next;
        SecondsLeft = CurrentIntervalNode.Value.IntervalLength;
        CurrentIntervalNr += 1;
        OnTimerStateChanged.Invoke();
    }

    public void TogglePlayPause()
    {
        TimerPaused = !TimerPaused;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_timer != null)
            await _timer.DisposeAsync();
    }

    public override string ToString()
    {
        var repr = "";
        repr += CurrentInterval + "\n\n";
        foreach (var interval in _intervals)
            repr += interval + "\n";
        return repr;
    }
}

