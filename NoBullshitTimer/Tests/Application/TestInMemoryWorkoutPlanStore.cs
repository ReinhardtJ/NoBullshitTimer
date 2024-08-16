using NoBullshitTimer.Client.Application;
using NoBullshitTimer.Client.Domain;
using NoBullshitTimer.Tests.Domain;
using NUnit.Framework;

namespace NoBullshitTimer.Tests.Application;

public class TestInMemoryWorkoutPlanStore
{
    private IWorkoutPlanStore _store;

    [SetUp]
    public void Init()
    {
        _store = new InMemoryWorkoutPlanStore();
    }
    [Test]
    public void Test_Add()
    {
        var workout = Fixtures.SomeWorkoutPlan();
        var addResult = _store.Add(workout, "SomeName");
        Assert.That(addResult, Is.True);
    }

    [Test]
    public void Test_AddExisting()
    {
        _store.Add(Fixtures.SomeWorkoutPlan(), "SomeName");
        var addResult =_store.Add(Fixtures.SomeWorkoutPlan(), "SomeName");
        Assert.That(addResult, Is.False);
    }

    [Test]
    public void Test_Get()
    {
        var workoutPlan = Fixtures.SomeWorkoutPlan();
        _store.Add(workoutPlan, "SomeName");
        var getResult = _store.GetByName("SomeName");
        Assert.That(getResult.Value(), Is.EqualTo(workoutPlan));
    }

    [Test]
    public void Test_GetNonExisting()
    {
        var getResult = _store.GetByName("SomeNonExistingName");
        Assert.That(getResult.IsNone());
    }
}
