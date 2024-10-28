namespace NoBullshitTimer.Client.Domain;

public abstract class Interval
{
    protected Interval(string name)
    {
        Name = name;
    }

    public abstract int IntervalLength { get; }

    public int SecondsIntoInterval(int secondsLeft)
    {
        return IntervalLength - secondsLeft;
    }

    public string Name { get; }

    public override string ToString()
    {
        return $"Interval {Name}: {{Interval Length: {IntervalLength}}}";
    }
}

public class Ready : Interval
{
    public Ready() : base("Ready")
    {
    }

    public override int IntervalLength => 0;
}

public class Prepare : Interval
{
    public Prepare(int prepareTime) : base("Prepare")
    {
        IntervalLength = prepareTime;
    }

    public override int IntervalLength { get; }
}

public class Work : Interval
{
    public Work(int workTime, string name) : base(name)
    {
        IntervalLength = workTime;
    }

    public override int IntervalLength { get; }
}

public class Rest : Interval
{
    public Rest(int restTime) : base("Rest")
    {
        IntervalLength = restTime;
    }

    public override int IntervalLength { get; }
}

public class Cooldown : Interval
{
    public Cooldown(int cooldownTime) : base("Cooldown")
    {
        IntervalLength = cooldownTime;
    }

    public override int IntervalLength { get; }
}

public class Done : Interval
{
    public Done() : base("Done")
    {
    }

    public override int IntervalLength => 0;
}
