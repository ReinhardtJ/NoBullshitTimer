using NoBullshitTimer.Client.Application;
using NoBullshitTimer.Tests.Domain;
using NUnit.Framework;

namespace NoBullshitTimer.Tests.Application;

public class TestInMemoryWorkoutStore
{
    private IWorkoutStore _store;

    [SetUp]
    public void Init()
    {
        _store = new InMemoryWorkoutStore();
    }
    [Test]
    public void Test_Add()
    {
        var workout = Fixtures.SomeWorkout();
        try
        {
            _store.Add(workout, "SomeName");
        }
        catch (Exception)
        {
            Assert.Fail();
        }
    }

    [Test]
    public void Test_AddExisting()
    {
        _store.Add(Fixtures.SomeWorkout(), "SomeName");
        Assert.Catch<AddingWorkoutException>(() => _store.Add(Fixtures.SomeWorkout(), "SomeName"));
    }

    [Test]
    public void Test_Get()
    {
        var workoutPlan = Fixtures.SomeWorkout();
        _store.Add(workoutPlan, "SomeName");
        var getResult = _store.Get("SomeName");
        Assert.That(getResult, Is.EqualTo(workoutPlan));
    }

    [Test]
    public void Test_GetNonExisting()
    {
        Assert.Catch<WorkoutNotFoundException>(() => _store.Get("SomeNonExistingName"));
    }
}
