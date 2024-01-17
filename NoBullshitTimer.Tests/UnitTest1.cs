namespace NoBullshitTimer.Tests;

using NoBullshitTimer.Domain;

public class Tests
{
    [SetUp]
    public void Setup()
    {
        var timer = new Timer();
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}