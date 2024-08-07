namespace NoBullshitTimer.Client.Framework;

public class TimeFormat
{
    /**
     * Parses a time of the format mm:ss to a TimeSpan object
     * e.g. 1:30 -> 90
     */
    public static TimeSpan ParseTime(string time, int defaultSeconds)
    {
        var parts = time.Split(':');
        if (parts.Length != 2)
            return TimeSpan.FromSeconds(defaultSeconds);
        if (parts[1].Length == 1)
            parts[1] += "0";
        if (parts[1].Length != 2)
            return TimeSpan.FromSeconds(defaultSeconds);
        if (int.TryParse(parts[0], out var minutes) && int.TryParse(parts[1], out var seconds))
            return new TimeSpan(0, minutes, seconds);

        return TimeSpan.FromSeconds(defaultSeconds);
    }
}
