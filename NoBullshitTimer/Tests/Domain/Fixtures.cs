using NoBullshitTimer.Client.Domain;

namespace NoBullshitTimer.Tests.Domain;

public static class Fixtures
{
    public static Workout SomeWorkout()
    {
        return new Workout(
            "SomeWorkout",
            TimeSpan.FromSeconds(10),
            TimeSpan.FromSeconds(40),
            TimeSpan.FromSeconds(20),
            TimeSpan.FromSeconds(60),
            2,
            new List<string> {"push ups", "pull ups"},
            false
        );
    }
}
