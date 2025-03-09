using Moq;
using NoBullshitTimer.Client.Application;
using NoBullshitTimer.Client.Domain;
using NoBullshitTimer.Client.Framework;
using NUnit.Framework;
using System.Text.Json;
using NoBullshitTimer.Client.Repositories;

namespace NoBullshitTimer.Tests.Application;

[TestFixture]
public class TestLocalStorageWorkoutRepository : TestWorkoutRepository
{
    private IWorkoutRepository _repository;
    protected override IWorkoutRepository Repository => _repository;

    private Mock<ILocalStorageService> _localStorageServiceMock;
    private Dictionary<string, string> _localStorageMock;

    [SetUp]
    public void Init()
    {
        _localStorageMock = new Dictionary<string, string>();
        _localStorageServiceMock = new Mock<ILocalStorageService>();

        // Setup mock localStorage behavior
        _localStorageServiceMock
            .Setup(service => service.GetItemAsync<Dictionary<Guid, Workout>>("workouts"))
            .ReturnsAsync(() =>
            {
                if (!_localStorageMock.ContainsKey("workouts"))
                    return new Dictionary<Guid, Workout>();
                return JsonSerializer.Deserialize<Dictionary<Guid, Workout>>(_localStorageMock["workouts"]);
            });

        _localStorageServiceMock
            .Setup(service => service.SetItemAsync("workouts", It.IsAny<string>()))
            .Callback<string, string>((key, value) => _localStorageMock[key] = value)
            .Returns(Task.CompletedTask);

        _repository = new LocalStorageWorkoutRepository(_localStorageServiceMock.Object);
    }
}
