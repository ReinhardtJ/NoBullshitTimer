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

    private Mock<ILocalStorageService> _mockLocalStorageService;
    private Dictionary<string, string> _localStorage;

    [SetUp]
    public void Init()
    {
        _localStorage = new Dictionary<string, string>();
        _mockLocalStorageService = new Mock<ILocalStorageService>();

        // Setup mock localStorage behavior
        _mockLocalStorageService
            .Setup(x => x.GetItemAsync<Dictionary<string, Workout>>("workouts"))
            .ReturnsAsync(() =>
            {
                if (!_localStorage.ContainsKey("workouts"))
                    return new Dictionary<string, Workout>();
                return JsonSerializer.Deserialize<Dictionary<string, Workout>>(_localStorage["workouts"]);
            });

        _mockLocalStorageService
            .Setup(x => x.SetItemAsync("workouts", It.IsAny<string>()))
            .Callback<string, string>((key, value) => _localStorage[key] = value)
            .Returns(Task.CompletedTask);

        _repository = new LocalStorageWorkoutRepository(_mockLocalStorageService.Object);
    }
}
