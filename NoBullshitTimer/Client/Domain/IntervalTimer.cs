using NoBullshitTimer.Client.Framework;

namespace NoBullshitTimer.Client.Domain;

public class IntervalTimer : IDisposable, IAsyncDisposable
{
    private readonly Action _onTickCallback;
    private Timer _timer;
    private LinkedListNode<Interval> CurrentIntervalNode { get; set; }

    public int CurrentIntervalNr { get; private set; }
    public int SecondsLeft { get; private set; }
    public bool TimerPaused = true;
    public WorkoutPlan WorkoutPlan { get; }
    private LinkedList<Interval> _intervals;

    public IntervalTimer(
        WorkoutPlan plan,
        Action onTickCallback
    )
    {
        _onTickCallback = onTickCallback;
        WorkoutPlan = plan;
        _intervals = new();
        _intervals.AddLast(new Ready());
        _intervals.AddLast(new Prepare(plan.PrepareTime.TotalSecondsInt()));
        foreach (var exercise in plan.Exercises)
        {
            for (var i = 0; i < plan.SetsPerExercise; i++)
            {
                _intervals.AddLast(new Work(plan.WorkTime.TotalSecondsInt(), exercise));
                if (i == plan.SetsPerExercise - 1 && exercise == plan.Exercises[^1])
                {
                    break;
                }

                _intervals.AddLast(new Rest(plan.RestTime.TotalSecondsInt()));
            }
        }
        Console.WriteLine(plan.CooldownTime);
        Console.WriteLine(plan.CooldownTime.TotalSecondsInt());
        _intervals.AddLast(new Cooldown(plan.CooldownTime.TotalSecondsInt()));
        _intervals.AddLast(new Done());
        CurrentIntervalNode = _intervals.First!;
        CurrentIntervalNr = 1;
        _timer = new Timer(_ =>
        {
            if (TimerPaused)
                return;
            Tick();
            _onTickCallback();
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
        _onTickCallback();
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
        _onTickCallback();
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

