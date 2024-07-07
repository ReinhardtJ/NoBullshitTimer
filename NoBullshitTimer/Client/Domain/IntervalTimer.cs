namespace NoBullshitTimer.Client.Domain;

public class IntervalTimer
{
    public int CurrentIntervalNr { get; private set; }
    public int SecondsLeft { get; private set; }
    public bool TimerPaused = true;
    public WorkoutPlan WorkoutPlan { get; }
    private LinkedListNode<Interval> CurrentIntervalNode { get; set; }

    public IntervalTimer(WorkoutPlan plan)
    {
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
    }

    public Interval CurrentInterval => CurrentIntervalNode.Value;
    public Interval? NextInterval => CurrentIntervalNode.Next?.Value;

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
        if (CurrentIntervalNode.Previous is null)
            return;

        if (CurrentIntervalNode.Value.SecondsIntoInterval(SecondsLeft) <= 5)
        {
            CurrentIntervalNode = CurrentIntervalNode.Previous;
            CurrentIntervalNr -= 1;
        }

        SecondsLeft = CurrentIntervalNode.Value.IntervalLength;
        if (CurrentInterval is Ready)
            TimerPaused = true;
    }

    public void GoToNextInterval()
    {
        if (CurrentIntervalNode.Next is null)
            return;

        CurrentIntervalNode = CurrentIntervalNode.Next;
        SecondsLeft = CurrentIntervalNode.Value.IntervalLength;
        CurrentIntervalNr += 1;
    }

    public void TogglePlayPause()
    {
        TimerPaused = !TimerPaused;
    }
}

