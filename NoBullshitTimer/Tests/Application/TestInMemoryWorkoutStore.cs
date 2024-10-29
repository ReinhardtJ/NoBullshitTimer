using NoBullshitTimer.Client.Application;
using NoBullshitTimer.Tests.Domain;
using NUnit.Framework;

namespace NoBullshitTimer.Tests.Application;

public class TestInMemoryWorkoutRepository
{
    private IWorkoutRepository _repository;

    [SetUp]
    public void Init()
    {
        _repository = new InMemoryWorkoutRepository();
    }
    [Test]
    public void Test_Add()
    {
        var workout = Fixtures.SomeWorkout();
        try
        {
            _repository.Add(workout, "SomeName");
        }
        catch (Exception)
        {
            Assert.Fail();
        }
    }

    [Test]
    public void Test_AddExisting()
    {
        _repository.Add(Fixtures.SomeWorkout(), "SomeName");
        Assert.Catch<AddingWorkoutException>(() => _repository.Add(Fixtures.SomeWorkout(), "SomeName"));
    }

    [Test]
    public void Test_Get()
    {
        var workoutPlan = Fixtures.SomeWorkout();
        _repository.Add(workoutPlan, "SomeName");
        var getResult = _repository.Get("SomeName");
        Assert.That(getResult, Is.EqualTo(workoutPlan));
    }

    [Test]
    public void Test_GetNonExisting()
    {
        Assert.Catch<WorkoutNotFoundException>(() => _repository.Get("SomeNonExistingName"));
    }
}
