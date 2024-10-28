using NoBullshitTimer.Client.Domain;

namespace NoBullshitTimer.Client.Framework;

public class Utils
{
    public static int ParseInt(string number, int defaultValue)
    {
        var sucess = int.TryParse(number, out var parsed);
        return sucess ? parsed : defaultValue;
    }
}