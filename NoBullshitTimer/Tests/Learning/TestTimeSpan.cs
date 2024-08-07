using NUnit.Framework;

namespace NoBullshitTimer.Tests.Learning;

public class TestTimeSpan
{
    [Test]
    public void TestTimeSpanToSeconds()
    {
        var timeSpan = TimeSpan.FromSeconds(30);
        Assert.That(timeSpan.Seconds, Is.EqualTo(30));
    }
}
