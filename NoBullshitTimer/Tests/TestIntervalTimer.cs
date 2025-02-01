using NoBullshitTimer.Client.Application;
using NoBullshitTimer.Client.Domain;
using NoBullshitTimer.Client.Repositories;
using NoBullshitTimer.Client.Stores;
using NoBullshitTimer.Tests.Domain;
using NUnit.Framework;

namespace NoBullshitTimer.Tests;

public class TestIntervalTimer
{
    private IntervalTimer _intervalTimer;

    [SetUp]
    public async Task Setup()
    {
        var workout = Fixtures.SomeWorkout();
        var workoutStore = await WorkoutStore.Create(new InMemoryWorkoutRepository());
        workoutStore.SelectedWorkout = workout;
        _intervalTimer = new IntervalTimer(workoutStore);
    }


    [Test]
    public async Task Test_Tick_paused_Timer_does_not_change_its_Interval()
    {
        Assert.That(_intervalTimer.CurrentInterval is Ready);
        await Task.Delay(1100);
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
        Assert.That(_intervalTimer.CurrentIntervalNr, Is.EqualTo(1));
        _intervalTimer.GoToNextInterval();
        Assert.That(_intervalTimer.CurrentIntervalNr, Is.EqualTo(2));
        _intervalTimer.GoToPreviousInterval();
        Assert.That(_intervalTimer.CurrentIntervalNr, Is.EqualTo(1));
    }

    [Test]
    public void Test_GoToPreviousInterval_goes_back_or_resets_Interval_depending_on_time_into_Interval()
    {
        _intervalTimer.GoToNextInterval();
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

        _intervalTimer.GoToNextInterval();
        Assert.That(_intervalTimer.CurrentInterval is Prepare);
        Assert.That(_intervalTimer.NextInterval is Work);
        Assert.That(_intervalTimer.NextInterval?.Name, Is.EqualTo("push ups"));

        _intervalTimer.GoToNextInterval();
        Assert.That(_intervalTimer.CurrentInterval is Work);
        Assert.That(_intervalTimer.CurrentInterval?.Name, Is.EqualTo("push ups"));
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

        _intervalTimer.GoToNextInterval();
        Assert.That(_intervalTimer.CurrentInterval is Done);
        Assert.That(_intervalTimer.NextInterval, Is.Null);
    }

    [Test]
    public void TestTimerStateMovingBackward()
    {
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
    public async Task TestTimerSecondsLeft()
    {
        var state = await WorkoutStore.Create(new InMemoryWorkoutRepository());
        state.SelectedWorkout = new Workout(
            "SomeWorkout",
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(3),
            TimeSpan.FromSeconds(2),
            TimeSpan.FromSeconds(1),
            1,
            new List<string> { "pull ups" },
            false
        );

        var intervalTimer = new IntervalTimer(state);
        Assert.That(intervalTimer.CurrentInterval is Ready);
        intervalTimer.Tick();
        Assert.That(intervalTimer.CurrentInterval is Prepare);
        Assert.That(intervalTimer.SecondsLeft, Is.EqualTo(1));
        intervalTimer.Tick();
        Assert.That(intervalTimer.CurrentInterval is Work);
        Assert.That(intervalTimer.SecondsLeft, Is.EqualTo(3));
    }

    [Test]
    public async Task TestCircularSets()
    {
        var workout = Fixtures.SomeWorkout(circularSets: true);
        var state = await WorkoutStore.Create(new InMemoryWorkoutRepository());
        state.SelectedWorkout = workout;
        var intervalTimer = new IntervalTimer(state); // ready
        intervalTimer.GoToNextInterval(); // prepare
        intervalTimer.GoToNextInterval(); // ex 1 set 1
        Assert.That(intervalTimer.CurrentInterval?.Name, Is.EqualTo("push ups"));
        intervalTimer.GoToNextInterval(); // rest
        intervalTimer.GoToNextInterval(); // ex 2 set 1
        Assert.That(intervalTimer.CurrentInterval.Name, Is.EqualTo("pull ups"));
        intervalTimer.GoToNextInterval(); // rest
        intervalTimer.GoToNextInterval(); // ex 1 set 2
        Assert.That(intervalTimer.CurrentInterval.Name, Is.EqualTo("push ups"));
        intervalTimer.GoToNextInterval(); // rest
        intervalTimer.GoToNextInterval(); // ex 2 set 2
        Assert.That(intervalTimer.CurrentInterval.Name, Is.EqualTo("pull ups"));
        intervalTimer.GoToNextInterval(); // cooldown
        Assert.That(intervalTimer.CurrentInterval is Cooldown);
    }

    private void Tick(IntervalTimer timer, int times)
    {
        for (var i = 0; i < times; i++)
            timer.Tick();
    }
}
