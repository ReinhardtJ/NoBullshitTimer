namespace NoBullshitTimer.Client.Framework;

public static class TimeSpanExtensions
{
    public static int TotalSecondsInt(this TimeSpan timeSpan)
    {
        return (int) timeSpan.TotalSeconds;
    }
}
