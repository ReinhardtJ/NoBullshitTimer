using NoBullshitTimer.Domain;
using NUnit.Framework;

namespace NoBullshitTimer.Tests;

public class TestIntervalTimer
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestMoveFromReadyToFirstInterval()
    {
        var intervalTimer = new IntervalTimer(5, 3, 2, ["push ups", "pull ups"]);
        Assert.That(intervalTimer.TimerState, Is.EqualTo(TimerState.Ready));
        intervalTimer.Tick();
        Assert.That(intervalTimer.TimerState, Is.EqualTo(TimerState.Ready));
        intervalTimer.PlayPause();
        intervalTimer.Tick();
        Assert.That(intervalTimer.TimerState, Is.EqualTo(TimerState.Work));
        Assert.That(intervalTimer.CurrentExercise, Is.EqualTo("push ups"));
        Assert.That(intervalTimer.NextExercise, Is.EqualTo("push ups"));
        Assert.That(intervalTimer.SecondsLeft, Is.EqualTo(4));
    }
}