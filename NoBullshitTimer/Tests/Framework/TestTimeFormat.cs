using NoBullshitTimer.Client.Framework;
using NUnit.Framework;

namespace NoBullshitTimer.Tests.Framework;

public class TestTimeFormat
{
    [Test]
    public void TestParseTime()
    {
        Assert.That(
            TimeFormat.ParseTime("", -1),
            Is.EqualTo(TimeSpan.FromSeconds(-1))
        );
        Assert.That(
            TimeFormat.ParseTime("0", -1),
            Is.EqualTo(TimeSpan.FromSeconds(-1))
        );
        Assert.That(
            TimeFormat.ParseTime("0:0", -1),
            Is.EqualTo(TimeSpan.FromSeconds(0))
        );
        Assert.That(
            TimeFormat.ParseTime("0:30", -1),
            Is.EqualTo(TimeSpan.FromSeconds(30))
        );
        Assert.That(
            TimeFormat.ParseTime("0:3", -1),
            Is.EqualTo(TimeSpan.FromSeconds(30))
        );
        Assert.That(
            TimeFormat.ParseTime("1:30", -1),
            Is.EqualTo(TimeSpan.FromSeconds(90))
        );
        Assert.That(
            TimeFormat.ParseTime("0:300", -1),
            Is.EqualTo(TimeSpan.FromSeconds(-1))
        );
        Assert.That(
            TimeFormat.ParseTime("01:30", -1),
            Is.EqualTo(TimeSpan.FromSeconds(90))
        );
        Assert.That(
            TimeFormat.ParseTime("001:30", -1),
            Is.EqualTo(TimeSpan.FromSeconds(90))
        );
        Assert.That(
            TimeFormat.ParseTime("10:30", -1),
            Is.EqualTo(TimeSpan.FromSeconds(630))
        );
        Assert.That(
            TimeFormat.ParseTime("010:30", -1),
            Is.EqualTo(TimeSpan.FromSeconds(630))
        );
        Assert.That(
            TimeFormat.ParseTime("01:00", -1),
            Is.EqualTo(TimeSpan.FromSeconds(60))
        );
    }
}
