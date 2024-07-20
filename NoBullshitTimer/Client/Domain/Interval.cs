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
    private readonly int _prepareTime;

    public Prepare(int prepareTime) : base("Prepare")
    {
        _prepareTime = prepareTime;
    }

    public override int IntervalLength => _prepareTime;
}

public class Work : Interval
{
    private readonly int _workTime;

    public Work(int workTime, string name) : base(name)
    {
        _workTime = workTime;
    }

    public override int IntervalLength => _workTime;
}

public class Rest : Interval
{
    private readonly int _restTime;

    public Rest(int restTime) : base("Rest")
    {
        _restTime = restTime;
    }

    public override int IntervalLength => _restTime;
}

public class Cooldown : Interval
{
    private readonly int _cooldownTime;

    public Cooldown(int cooldownTime) : base("Cooldown")
    {
        _cooldownTime = cooldownTime;
    }

    public override int IntervalLength => _cooldownTime;
}

public class Done : Interval
{
    public Done() : base("Done")
    {
    }

    public override int IntervalLength => 0;
}
