using NoBullshitTimer.Client.Domain;
using NoBullshitTimer.Client.Repositories;
using NUnit.Framework;

namespace NoBullshitTimer.Tests.Application;

public class TestInMemoryWorkoutRepository : TestWorkoutRepository
{
    private IWorkoutRepository _repository;

    protected override IWorkoutRepository Repository => _repository;

    [SetUp]
    public void Init()
    {
        _repository = new InMemoryWorkoutRepository(
            new Dictionary<Guid, Workout>()
        );
    }
}
