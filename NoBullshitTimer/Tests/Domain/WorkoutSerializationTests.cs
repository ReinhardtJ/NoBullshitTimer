using System.Text.Json;
using NoBullshitTimer.Client.Domain;
using NUnit.Framework;

namespace NoBullshitTimer.Tests.Domain;

[TestFixture]
public class WorkoutSerializationTests
{
    [Test]
    public void JsonSerializationRoundtrip_PreservesAllProperties()
    {
        var originalWorkout = Fixtures.SomeWorkout();

        var jsonString = JsonSerializer.Serialize(originalWorkout);
        var deserializedWorkout = JsonSerializer.Deserialize<Workout>(jsonString);

        Assert.That(deserializedWorkout, Is.Not.Null);
        Assert.That(deserializedWorkout, Is.EqualTo(originalWorkout));
    }
}
