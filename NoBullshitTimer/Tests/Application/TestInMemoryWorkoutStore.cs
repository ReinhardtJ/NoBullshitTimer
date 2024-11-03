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
            _repository.Add(workout);
        }
        catch (Exception)
        {
            Assert.Fail();
        }
    }

    [Test]
    public void Test_AddExisting()
    {
        _repository.Add(Fixtures.SomeWorkout());
        Assert.Catch<AddingWorkoutException>(() => _repository.Add(Fixtures.SomeWorkout()));
    }

    [Test]
    public void Test_Get()
    {
        var workout = Fixtures.SomeWorkout();
        _repository.Add(workout);
        var getResult = _repository.Get(workout.Name);
        Assert.That(getResult, Is.EqualTo(workout));
    }

    [Test]
    public void Test_GetNonExisting()
    {
        Assert.Catch<WorkoutNotFoundException>(() => _repository.Get("SomeNonExistingName"));
    }
}
