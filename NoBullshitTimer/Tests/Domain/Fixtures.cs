using NoBullshitTimer.Client.Domain;

namespace NoBullshitTimer.Tests.Domain;

public static class Fixtures
{
    public static Workout SomeWorkout(bool circularSets = false)
    {
        return new Workout(
            new Guid("9395df3b-e341-49a7-b5d9-0cbd78bfc4be"),
            "SomeWorkout",
            TimeSpan.FromSeconds(10),
            TimeSpan.FromSeconds(40),
            TimeSpan.FromSeconds(20),
            TimeSpan.FromSeconds(60),
            2,
            new List<string> {"push ups", "pull ups"},
            circularSets
        );
    }
}
