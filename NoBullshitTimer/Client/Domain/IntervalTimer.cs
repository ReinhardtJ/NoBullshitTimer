namespace NoBullshitTimer.Domain;

public abstract class TimeInterval(string name)
{
    public abstract int IntervalLength { get; }

    public int SecondsIntoInterval(int secondsLeft)
    {
        return IntervalLength - secondsLeft;
    }

    public string Name { get; } = name;
}

public class Ready() : TimeInterval("Ready")
{
    public override int IntervalLength => 0;
}

public class Prepare(int prepareTime) : TimeInterval("Prepare")
{
    public override int IntervalLength => prepareTime;
}

public class Work(
    int workTime,
    bool isFirstInterval,
    bool isLastInterval,
    string name
) : TimeInterval(name)
{
    public override int IntervalLength => workTime;
}

public class Rest(int restTime) : TimeInterval("Rest")
{
    public override int IntervalLength => restTime;
}

public class Cooldown(int cooldownTime) : TimeInterval("Cooldown")
{
    public override int IntervalLength => cooldownTime;
}

public class Done() : TimeInterval("Done")
{
    public override int IntervalLength => 0;
}


public class IntervalTimer
{

    private LinkedList<TimeInterval> _intervals;

    public bool TimerPaused = true;
    public int TotalIntervals => _intervals.Count;

    // tracked variables
    public int Interval { get; private set; }
    private readonly int _exerciseIndex = 0;

    public IntervalTimer(
        int prepareTime,
        int workTime,
        int restTime,
        int cooldownTime,
        int intervals,
        List<string> exercises
    )
    {
        _intervals = new LinkedList<TimeInterval>();
        _intervals.AddLast(new Ready());
        _intervals.AddLast(new Prepare(prepareTime));
        foreach (var exercise in exercises)
        {
            for (var i = 0; i < intervals; i++)
            {
                _intervals.AddLast(new Work(workTime, i == 0, i == intervals - 1, exercise));
                if (i == intervals - 1 && exercise == exercises[^1])
                {
                    break;
                }

                _intervals.AddLast(new Rest(restTime));
            }
        }

        _intervals.AddLast(new Cooldown(cooldownTime));
        _intervals.AddLast(new Done());
        CurrentIntervalNode = _intervals.First;
    }



    // computed variables
    // public int TotalIntervalsDone => _interval - 1;
    // private int IntervalsDoneInExercise => Interval - _exercises.Count * (_exerciseIndex - 1) - 1;

    public int SecondsLeft { get; private set; }
    private LinkedListNode<TimeInterval?> CurrentIntervalNode { get; set; }
    public TimeInterval? CurrentInterval => CurrentIntervalNode.Value;
    public TimeInterval? NextInterval => CurrentIntervalNode.Next?.Value;

    public void Tick()
    {
        if (TimerPaused || CurrentIntervalNode.Value is Done)
            return;

        SecondsLeft -= 1;

        if (SecondsLeft <= 0)
            GoToNextInterval();
    }

    public void GoToPreviousInterval()
    {
        if (CurrentIntervalNode.Value.SecondsIntoInterval(SecondsLeft) < 5)
        {
            CurrentIntervalNode = CurrentIntervalNode.Previous;
        }
        SecondsLeft = CurrentIntervalNode.Value.IntervalLength;
        if (CurrentInterval is Ready)
        {
            TimerPaused = true;
        }
    }

    public void GoToNextInterval()
    {
        CurrentIntervalNode = CurrentIntervalNode.Next;
        SecondsLeft = CurrentIntervalNode.Value.IntervalLength;
    }

    public void PlayPause()
    {
        TimerPaused = !TimerPaused;
    }
}

