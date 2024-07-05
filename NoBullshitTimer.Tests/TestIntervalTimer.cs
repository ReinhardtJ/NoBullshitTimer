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
    public void TestTickingPausedTimerDoesNotChangeState()
    {
        var intervalTimer = new IntervalTimer(10, 40, 20, 60, 2, ["push ups", "pull ups"]);
        Assert.That(intervalTimer.CurrentInterval is Ready);
        intervalTimer.Tick();
        Assert.That(intervalTimer.CurrentInterval is Ready);
    }

    [Test]
    public void TestTogglePlayPause()
    {
        var intervalTimer = new IntervalTimer(10, 40, 20, 60, 2, ["push ups", "pull ups"]);
        Assert.That(intervalTimer.TimerPaused, Is.True);
        intervalTimer.PlayPause();
        Assert.That(intervalTimer.TimerPaused, Is.False);
        intervalTimer.PlayPause();
        Assert.That(intervalTimer.TimerPaused, Is.True);
    }

    [Test]
    public void TestTimerStateMovingForward()
    {
        var intervalTimer = new IntervalTimer(10, 40, 20, 60, 2, ["push ups", "pull ups"]);
        Assert.That(intervalTimer.CurrentInterval is Ready);
        Assert.That(intervalTimer.NextInterval is Prepare);
        intervalTimer.PlayPause();

        intervalTimer.GoToNextInterval();
        Assert.That(intervalTimer.CurrentInterval is Prepare);
        Assert.That(intervalTimer.NextInterval is Work);
        Assert.That(intervalTimer.NextInterval.Name, Is.EqualTo("push ups"));

        intervalTimer.GoToNextInterval();
        Assert.That(intervalTimer.CurrentInterval is Work);
        Assert.That(intervalTimer.CurrentInterval.Name, Is.EqualTo("push ups"));
        Assert.That(intervalTimer.NextInterval is Rest);

        intervalTimer.GoToNextInterval();
        Assert.That(intervalTimer.CurrentInterval is Rest);
        Assert.That(intervalTimer.NextInterval.Name, Is.EqualTo("push ups"));
        Assert.That(intervalTimer.NextInterval is Work);

        intervalTimer.GoToNextInterval();
        Assert.That(intervalTimer.CurrentInterval is Work);
        Assert.That(intervalTimer.CurrentInterval.Name, Is.EqualTo("push ups"));
        Assert.That(intervalTimer.NextInterval is Rest);

        intervalTimer.GoToNextInterval();
        Assert.That(intervalTimer.CurrentInterval is Rest);
        Assert.That(intervalTimer.NextInterval is Work);
        Assert.That(intervalTimer.NextInterval.Name, Is.EqualTo("pull ups"));

        intervalTimer.GoToNextInterval();
        Assert.That(intervalTimer.CurrentInterval is Work);
        Assert.That(intervalTimer.CurrentInterval.Name, Is.EqualTo("pull ups"));
        Assert.That(intervalTimer.NextInterval is Rest);

        intervalTimer.GoToNextInterval();
        Assert.That(intervalTimer.CurrentInterval is Rest);
        Assert.That(intervalTimer.NextInterval.Name, Is.EqualTo("pull ups"));
        Assert.That(intervalTimer.NextInterval is Work);

        intervalTimer.GoToNextInterval();
        Assert.That(intervalTimer.CurrentInterval is Work);
        Assert.That(intervalTimer.CurrentInterval.Name, Is.EqualTo("pull ups"));
        Assert.That(intervalTimer.NextInterval is Cooldown);

        intervalTimer.GoToNextInterval();
        Assert.That(intervalTimer.CurrentInterval is Cooldown);
        Assert.That(intervalTimer.NextInterval is Done);

        intervalTimer.GoToNextInterval();
        Assert.That(intervalTimer.CurrentInterval is Done);
        Assert.That(intervalTimer.NextInterval, Is.Null);
    }

    [Test]
    public void TestTimerStateMovingBackward()
    {
        var intervalTimer = new IntervalTimer(10, 40, 20, 60, 2, ["push ups", "pull ups"]);
        intervalTimer.PlayPause();
        while (intervalTimer.CurrentInterval is not Done)
        {
            intervalTimer.GoToNextInterval();
        }
        Assert.That(intervalTimer.CurrentInterval is Done);
        intervalTimer.GoToPreviousInterval();
        Assert.That(intervalTimer.CurrentInterval is Cooldown);
        intervalTimer.GoToPreviousInterval();
        Assert.That(intervalTimer.CurrentInterval is Work);
        intervalTimer.GoToPreviousInterval();
        Assert.That(intervalTimer.CurrentInterval is Rest);
        intervalTimer.GoToPreviousInterval();
        Assert.That(intervalTimer.CurrentInterval is Work);
        intervalTimer.GoToPreviousInterval();
        Assert.That(intervalTimer.CurrentInterval is Rest);
        intervalTimer.GoToPreviousInterval();
        Assert.That(intervalTimer.CurrentInterval is Work);
        intervalTimer.GoToPreviousInterval();
        Assert.That(intervalTimer.CurrentInterval is Rest);
        intervalTimer.GoToPreviousInterval();
        Assert.That(intervalTimer.CurrentInterval is Work);
        intervalTimer.GoToPreviousInterval();
        Assert.That(intervalTimer.CurrentInterval is Prepare);
        intervalTimer.GoToPreviousInterval();
        Assert.That(intervalTimer.CurrentInterval is Ready);
    }

    [Test]
    public void TestTimerSecondsLeft()
    {
        var intervalTimer = new IntervalTimer(
            1,
            3,
            2,
            1,
            1,
            ["pull ups"]
        );

        intervalTimer.PlayPause();
        Assert.That(intervalTimer.CurrentInterval is Ready);
        intervalTimer.Tick();
        Assert.That(intervalTimer.CurrentInterval is Prepare);
        Assert.That(intervalTimer.SecondsLeft, Is.EqualTo(1));
        intervalTimer.Tick();
        Assert.That(intervalTimer.CurrentInterval is Work);
        Assert.That(intervalTimer.SecondsLeft is 3);
}
}
