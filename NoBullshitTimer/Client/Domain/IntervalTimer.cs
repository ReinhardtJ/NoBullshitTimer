using NoBullshitTimer.Client.Application;
using NoBullshitTimer.Client.Framework;

namespace NoBullshitTimer.Client.Domain;

public class IntervalTimer : IDisposable, IAsyncDisposable
{

    private Timer _timer;
    private LinkedListNode<Interval> CurrentIntervalNode { get; set; }

    public event Action OnTimerTick = () => { };

    public int CurrentIntervalNr { get; private set; }
    public int SecondsLeft { get; private set; }
    public bool TimerPaused = true;
    public Workout Workout { get; set; }
    private LinkedList<Interval> _intervals;

    public IntervalTimer(IWorkoutState workoutState)
    {
        workoutState.OnWorkoutChanged += InitIntervalTimer;
        InitIntervalTimer(workoutState.Workout);
    }

    private void InitIntervalTimer(Workout workout)
    {
        Workout = workout;
        _intervals = new();
        _intervals.AddLast(new Ready());
        _intervals.AddLast(new Prepare(workout.PrepareTime.TotalSecondsInt()));
        foreach (var exercise in workout.Exercises)
        {
            for (var i = 0; i < workout.SetsPerExercise; i++)
            {
                _intervals.AddLast(new Work(workout.ExerciseTime.TotalSecondsInt(), exercise));
                if (i == workout.SetsPerExercise - 1 && exercise == workout.Exercises[^1])
                {
                    break;
                }

                _intervals.AddLast(new Rest(workout.RestTime.TotalSecondsInt()));
            }
        }
        Console.WriteLine(workout.CooldownTime);
        Console.WriteLine(workout.CooldownTime.TotalSecondsInt());
        _intervals.AddLast(new Cooldown(workout.CooldownTime.TotalSecondsInt()));
        _intervals.AddLast(new Done());
        CurrentIntervalNode = _intervals.First!;
        CurrentIntervalNr = 1;
        _timer = new Timer(_ =>
        {
            if (TimerPaused)
                return;
            Tick();
            OnTimerTick.Invoke();
        }, null, 0L, 1000L);
    }

    public Interval CurrentInterval => CurrentIntervalNode.Value;
    public Interval? NextInterval => CurrentIntervalNode.Next?.Value;

    public void Tick()
    {
        SecondsLeft -= 1;

        if (SecondsLeft <= 0)
            GoToNextInterval();
    }

    public void GoToPreviousInterval()
    {
        if (CurrentIntervalNode.Previous is null) return;

        if (CurrentIntervalNode.Value.SecondsIntoInterval(SecondsLeft) <= 5)
        {
            CurrentIntervalNode = CurrentIntervalNode.Previous;
            CurrentIntervalNr -= 1;
        }

        SecondsLeft = CurrentIntervalNode.Value.IntervalLength;
        if (CurrentInterval is Ready)
            TimerPaused = true;
        OnTimerTick.Invoke();
    }

    public void GoToNextInterval()
    {
        if (CurrentIntervalNode.Next is null)
        {
            return;
        }
        if (NextInterval is Done)
        {
            TimerPaused = true;
        }

        CurrentIntervalNode = CurrentIntervalNode.Next;
        SecondsLeft = CurrentIntervalNode.Value.IntervalLength;
        CurrentIntervalNr += 1;
        OnTimerTick.Invoke();
    }

    public void TogglePlayPause()
    {
        TimerPaused = !TimerPaused;
    }

    public void Dispose()
    {
        _timer.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _timer.DisposeAsync();
    }

    public override string ToString()
    {
        var repr = "";
        repr += CurrentInterval + "\n\n";
        foreach (var interval in _intervals)
        {
            repr += interval + "\n";
        }
        return repr;
    }
}

