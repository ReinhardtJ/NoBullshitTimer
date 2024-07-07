namespace NoBullshitTimer.Client.Domain;

public abstract class Interval(string name)
{
    public abstract int IntervalLength { get; }

    public int SecondsIntoInterval(int secondsLeft)
    {
        return IntervalLength - secondsLeft;
    }

    public string Name { get; } = name;
}

public class Ready() : Interval("Ready")
{
    public override int IntervalLength => 0;
}

public class Prepare(int prepareTime) : Interval("Prepare")
{
    public override int IntervalLength => prepareTime;
}

public class Work(int workTime, string name) : Interval(name)
{
    public override int IntervalLength => workTime;
}

public class Rest(int restTime) : Interval("Rest")
{
    public override int IntervalLength => restTime;
}

public class Cooldown(int cooldownTime) : Interval("Cooldown")
{
    public override int IntervalLength => cooldownTime;
}

public class Done() : Interval("Done")
{
    public override int IntervalLength => 0;
}
