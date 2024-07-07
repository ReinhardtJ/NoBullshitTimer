using NoBullshitTimer.Client.Domain;
using NUnit.Framework;

namespace NoBullshitTimer.Tests;

public class TestIntervalTimer
{
    private IntervalTimer _intervalTimer;

    [SetUp]
    public void Setup()
    {
        var workoutPlan = new WorkoutPlan(
            10,
            40,
            20,
            60,
            2,
            ["push ups", "pull ups"]
        );
        _intervalTimer = new IntervalTimer(workoutPlan);
    }


    [Test]
    public void Test_Tick_paused_Timer_does_not_change_its_Interval()
    {
        Assert.That(_intervalTimer.CurrentInterval is Ready);
        _intervalTimer.Tick();
        Assert.That(_intervalTimer.CurrentInterval is Ready);
    }

    [Test]
    public void Test_TogglePlayPause()
    {
        Assert.That(_intervalTimer.TimerPaused, Is.True);
        _intervalTimer.TogglePlayPause();
        Assert.That(_intervalTimer.TimerPaused, Is.False);
        _intervalTimer.TogglePlayPause();
        Assert.That(_intervalTimer.TimerPaused, Is.True);
    }

    [Test]
    public void Test_Interval_gets_tracked_correctly()
    {
        Assert.That(_intervalTimer.CurrentIntervalNr, Is.EqualTo(0));
        _intervalTimer.GoToNextInterval();
        Assert.That(_intervalTimer.CurrentIntervalNr, Is.EqualTo(1));
        _intervalTimer.GoToPreviousInterval();
        Assert.That(_intervalTimer.CurrentIntervalNr, Is.EqualTo(0));
    }

    [Test]
    public void Test_GoToPreviousInterval_goes_back_or_resets_Interval_depending_on_time_into_Interval()
    {
        _intervalTimer.GoToNextInterval();
        _intervalTimer.TogglePlayPause();
        Tick(_intervalTimer, 6);
        _intervalTimer.GoToPreviousInterval();
        Assert.That(_intervalTimer.CurrentInterval is Prepare);
        Assert.That(_intervalTimer.SecondsLeft, Is.EqualTo(10));
        Tick(_intervalTimer, 5);
        _intervalTimer.GoToPreviousInterval();
        Assert.That(_intervalTimer.CurrentInterval is Ready);
    }

    [Test]
    public void TestGoingBackFromReadyDoesNothing()
    {
        while (_intervalTimer.CurrentInterval is not Done)
        {
            _intervalTimer.GoToNextInterval();
        }
        Assert.That(_intervalTimer.CurrentInterval is Done);
        _intervalTimer.GoToNextInterval();
        Assert.That(_intervalTimer.CurrentInterval is Done);
    }

    [Test]
    public void TestGoingForwardFromNoneDoesNothing()
    {
        Assert.That(_intervalTimer.CurrentInterval is Ready);
        _intervalTimer.GoToPreviousInterval();
        Assert.That(_intervalTimer.CurrentInterval is Ready);

    }

    [Test]
    public void TestTimerStateMovingForward()
    {
        Assert.That(_intervalTimer.CurrentInterval is Ready);
        Assert.That(_intervalTimer.NextInterval is Prepare);
        _intervalTimer.TogglePlayPause();

        _intervalTimer.GoToNextInterval();
        Assert.That(_intervalTimer.CurrentInterval is Prepare);
        Assert.That(_intervalTimer.NextInterval is Work);
        Assert.That(_intervalTimer.NextInterval.Name, Is.EqualTo("push ups"));

        _intervalTimer.GoToNextInterval();
        Assert.That(_intervalTimer.CurrentInterval is Work);
        Assert.That(_intervalTimer.CurrentInterval.Name, Is.EqualTo("push ups"));
        Assert.That(_intervalTimer.NextInterval is Rest);

        _intervalTimer.GoToNextInterval();
        Assert.That(_intervalTimer.CurrentInterval is Rest);
        Assert.That(_intervalTimer.NextInterval.Name, Is.EqualTo("push ups"));
        Assert.That(_intervalTimer.NextInterval is Work);

        _intervalTimer.GoToNextInterval();
        Assert.That(_intervalTimer.CurrentInterval is Work);
        Assert.That(_intervalTimer.CurrentInterval.Name, Is.EqualTo("push ups"));
        Assert.That(_intervalTimer.NextInterval is Rest);

        _intervalTimer.GoToNextInterval();
        Assert.That(_intervalTimer.CurrentInterval is Rest);
        Assert.That(_intervalTimer.NextInterval is Work);
        Assert.That(_intervalTimer.NextInterval.Name, Is.EqualTo("pull ups"));

        _intervalTimer.GoToNextInterval();
        Assert.That(_intervalTimer.CurrentInterval is Work);
        Assert.That(_intervalTimer.CurrentInterval.Name, Is.EqualTo("pull ups"));
        Assert.That(_intervalTimer.NextInterval is Rest);

        _intervalTimer.GoToNextInterval();
        Assert.That(_intervalTimer.CurrentInterval is Rest);
        Assert.That(_intervalTimer.NextInterval.Name, Is.EqualTo("pull ups"));
        Assert.That(_intervalTimer.NextInterval is Work);

        _intervalTimer.GoToNextInterval();
        Assert.That(_intervalTimer.CurrentInterval is Work);
        Assert.That(_intervalTimer.CurrentInterval.Name, Is.EqualTo("pull ups"));
        Assert.That(_intervalTimer.NextInterval is Cooldown);

        _intervalTimer.GoToNextInterval();
        Assert.That(_intervalTimer.CurrentInterval is Cooldown);
        Assert.That(_intervalTimer.NextInterval is Done);

        _intervalTimer.GoToNextInterval();
        Assert.That(_intervalTimer.CurrentInterval is Done);
        Assert.That(_intervalTimer.NextInterval, Is.Null);
    }

    [Test]
    public void TestTimerStateMovingBackward()
    {
        _intervalTimer.TogglePlayPause();
        while (_intervalTimer.CurrentInterval is not Done)
        {
            _intervalTimer.GoToNextInterval();
        }
        Assert.That(_intervalTimer.CurrentInterval is Done);
        _intervalTimer.GoToPreviousInterval();
        Assert.That(_intervalTimer.CurrentInterval is Cooldown);
        _intervalTimer.GoToPreviousInterval();
        Assert.That(_intervalTimer.CurrentInterval is Work);
        _intervalTimer.GoToPreviousInterval();
        Assert.That(_intervalTimer.CurrentInterval is Rest);
        _intervalTimer.GoToPreviousInterval();
        Assert.That(_intervalTimer.CurrentInterval is Work);
        _intervalTimer.GoToPreviousInterval();
        Assert.That(_intervalTimer.CurrentInterval is Rest);
        _intervalTimer.GoToPreviousInterval();
        Assert.That(_intervalTimer.CurrentInterval is Work);
        _intervalTimer.GoToPreviousInterval();
        Assert.That(_intervalTimer.CurrentInterval is Rest);
        _intervalTimer.GoToPreviousInterval();
        Assert.That(_intervalTimer.CurrentInterval is Work);
        _intervalTimer.GoToPreviousInterval();
        Assert.That(_intervalTimer.CurrentInterval is Prepare);
        _intervalTimer.GoToPreviousInterval();
        Assert.That(_intervalTimer.CurrentInterval is Ready);
    }

    [Test]
    public void TestTimerSecondsLeft()
    {
        var intervalTimer = new IntervalTimer(new WorkoutPlan(
            1,
            3,
            2,
            1,
            1,
            ["pull ups"]
        ));
        intervalTimer.TogglePlayPause();
        Assert.That(intervalTimer.CurrentInterval is Ready);
        intervalTimer.Tick();
        Assert.That(intervalTimer.CurrentInterval is Prepare);
        Assert.That(intervalTimer.SecondsLeft, Is.EqualTo(1));
        intervalTimer.Tick();
        Assert.That(intervalTimer.CurrentInterval is Work);
        Assert.That(intervalTimer.SecondsLeft is 3);
    }

    private void Tick(IntervalTimer timer, int times)
    {
        for (var i = 0; i < times; i++)
            timer.Tick();
    }
}
