using NoBullshitTimer.Client.Repositories;
using NoBullshitTimer.Tests.Domain;
using NUnit.Framework;

namespace NoBullshitTimer.Tests.Application;

public abstract class TestWorkoutRepository
{
    protected abstract IWorkoutRepository Repository { get; }

    [Test]
    public async Task Test_Add()
    {
        var workout = Fixtures.SomeWorkout();
        try
        {
            await Repository.Add(workout);
        }
        catch (Exception)
        {
            Assert.Fail();
        }
    }

    [Test]
    public async Task Test_AddExisting()
    {
        await Repository.Add(Fixtures.SomeWorkout());
        Assert.ThrowsAsync<AddingWorkoutException>(async () => await Repository.Add(Fixtures.SomeWorkout()));
    }

    [Test]
    public async Task Test_Get()
    {
        var workout = Fixtures.SomeWorkout();
        await Repository.Add(workout);
        var getResult = await Repository.Get(workout.Id);
        Assert.That(getResult, Is.EqualTo(workout));
    }

    [Test]
    public void Test_GetNonExisting()
    {
        Assert.ThrowsAsync<WorkoutNotFoundException>(async () => await Repository.Get(Guid.NewGuid()));
    }

    [Test]
    public async Task Test_Delete()
    {
        var someWorkout = Fixtures.SomeWorkout();
        await Repository.Add(someWorkout);
        await Repository.Delete(someWorkout.Id);
        Assert.That(await Repository.GetAllWorkouts(), Is.Empty);
    }

    [Test]
    public async Task Test_Delete_DoesNothingIfWorkoutDoesNotExist()
    {
        await Repository.Delete(Guid.NewGuid());
        Assert.That(await Repository.GetAllWorkouts(), Is.Empty);
    }

    [Test]
    public async Task Test_OnRepositoryChanged_InvokedOnAdd()
    {
        var wasInvoked = false;
        Repository.OnRepositoryChanged += () =>
        {
            wasInvoked = true;
            return Task.CompletedTask;
        };

        await Repository.Add(Fixtures.SomeWorkout());
        Assert.That(wasInvoked, Is.True);
    }

    [Test]
    public async Task Test_OnRepositoryChanged_NotInvokedOnDeleteNonExisting()
    {
        var wasInvoked = false;
        Repository.OnRepositoryChanged += () =>
        {
            wasInvoked = true;
            return Task.CompletedTask;
        };

        await Repository.Delete(Guid.NewGuid());
        Assert.That(wasInvoked, Is.False);
    }

    [Test]
    public async Task Test_OnRepositoryChanged_InvokedOnDelete()
    {
        var wasInvoked = false;
        Repository.OnRepositoryChanged += () =>
        {
            wasInvoked = true;
            return Task.CompletedTask;
        };

        var workout = Fixtures.SomeWorkout();
        await Repository.Add(workout);
        await Repository.Delete(workout.Id);
        Assert.That(wasInvoked, Is.True);
    }
}