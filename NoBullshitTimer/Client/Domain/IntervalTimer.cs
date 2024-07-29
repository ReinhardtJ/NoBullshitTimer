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


    public IntervalTimer(
        WorkoutPlan plan,
        Action onTickCallback
    )
    {
        _onTickCallback = onTickCallback;
        WorkoutPlan = plan;
        LinkedList<Interval> intervals = new();
        intervals.AddLast(new Ready());
        intervals.AddLast(new Prepare(plan.prepareTime));
        foreach (var exercise in plan.exercises)
        {
            for (var i = 0; i < plan.setsPerExercise; i++)
            {
                intervals.AddLast(new Work(plan.workTime, exercise));
                if (i == plan.setsPerExercise - 1 && exercise == plan.exercises[^1])
                {
                    break;
                }

                intervals.AddLast(new Rest(plan.restTime));
            }
        }

        intervals.AddLast(new Cooldown(plan.cooldownTime));
        intervals.AddLast(new Done());
        CurrentIntervalNode = intervals.First!;
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
}

