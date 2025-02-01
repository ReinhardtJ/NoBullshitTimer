using System.Text.Json;
using NoBullshitTimer.Client.Domain;
using NoBullshitTimer.Client.Framework;
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

    [Test]
    public void Should_Deserialize_Json_To_Dictionary_Of_Workouts()
    {
        // Arrange
        var workouts = new Dictionary<string, Workout>
        {
            ["HIIT"] = WorkoutPresets.HIITPreset(),
            ["Tabata"] = WorkoutPresets.TabataPreset()
        };

        // JSRuntime double-encodes json, so we do it here as well
        var serialized = JsonSerializer.Serialize(workouts);
        var doubleSerialized = JsonSerializer.Serialize(serialized);
        JsonElement jsonElement;
        using JsonDocument doc = JsonDocument.Parse(doubleSerialized);
        jsonElement = doc.RootElement.Clone();


        // Act & Assert
        Assert.DoesNotThrow(() =>
        {
            var result = LocalStorageService.DeserializeJsonElementAsync<Dictionary<string, Workout>>(jsonElement);

            // Verify dictionary
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Count, Is.EqualTo(2));

            // Verify HIIT workout
            Assert.That(result.ContainsKey("HIIT"), Is.True);
            var hiit = result["HIIT"];
            Assert.That(hiit, Is.EqualTo(WorkoutPresets.HIITPreset()));
        });
    }
}
