using NoBullshitTimer.Client.Framework;
using NUnit.Framework;

namespace NoBullshitTimer.Tests.Framework;

public class TestCollectionExtensions
{
    [Test]
    public void SwapToFront_EmptyList_DoesNotThrow()
    {
        var list = new List<int>();
        Assert.DoesNotThrow(() => list.SwapToFront(1));
    }

    [Test]
    public void SwapToFront_SingleElement_DoesNothing()
    {
        var list = new List<int> { 1 };
        list.SwapToFront(1);
        Assert.That(list, Is.EqualTo(new[] { 1 }));
    }

    [Test]
    public void SwapToFront_TwoElements_SwapsCorrectly()
    {
        var list = new List<int> { 1, 2 };
        list.SwapToFront(2);
        Assert.That(list, Is.EqualTo(new[] { 2, 1 }));
    }

    [Test]
    public void SwapToFront_ThreeElements_SwapsCorrectly()
    {
        var list = new List<int> { 1, 2, 3 };
        list.SwapToFront(2);
        Assert.That(list, Is.EqualTo(new[] { 2, 1, 3 }));
    }

    [Test]
    public void SwapToFront_FirstElement_DoesNothing()
    {
        var list = new List<int> { 1, 2, 3 };
        list.SwapToFront(1);
        Assert.That(list, Is.EqualTo(new[] { 1, 2, 3 }));
    }

    [Test]
    public void SwapToBack_EmptyList_DoesNotThrow()
    {
        var list = new List<int>();
        Assert.DoesNotThrow(() => list.SwapToBack(1));
    }

    [Test]
    public void SwapToBack_SingleElement_DoesNothing()
    {
        var list = new List<int> { 1 };
        list.SwapToBack(1);
        Assert.That(list, Is.EqualTo(new[] { 1 }));
    }

    [Test]
    public void SwapToBack_TwoElements_SwapsCorrectly()
    {
        var list = new List<int> { 1, 2 };
        list.SwapToBack(1);
        Assert.That(list, Is.EqualTo(new[] { 2, 1 }));
    }

    [Test]
    public void SwapToBack_ThreeElements_SwapsCorrectly()
    {
        var list = new List<int> { 1, 2, 3 };
        list.SwapToBack(2);
        Assert.That(list, Is.EqualTo(new[] { 1, 3, 2 }));
    }

    [Test]
    public void SwapToBack_LastElement_DoesNothing()
    {
        var list = new List<int> { 1, 2, 3 };
        list.SwapToBack(3);
        Assert.That(list, Is.EqualTo(new[] { 1, 2, 3 }));
    }
}