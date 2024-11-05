namespace NoBullshitTimer.Client.Framework;

public static class CollectionExtensions
{
    /// <summary>
    /// Swaps the given element with the element in the list located IN FRONT.
    /// Does nothing when the given element is already the first element
    /// </summary>
    /// <param name="list"></param>
    /// <param name="elementToSwap"></param>
    /// <typeparam name="T"></typeparam>
    public static void SwapToFront<T>(this IList<T> list, T elementToSwap)
    {
        var index = list.IndexOf(elementToSwap);
        if (index <= 0)
            return;
        var previous = list[index - 1];
        list[index] = previous;
        list[index - 1] = elementToSwap;
    }

    /// <summary>
    /// Swaps the given element with the element in the list located BEHIND.
    /// Does nothing when the given element is already the last element
    /// </summary>
    /// <param name="list"></param>
    /// <param name="elementToSwap"></param>
    /// <typeparam name="T"></typeparam>
    public static void SwapToBack<T>(this IList<T> list, T elementToSwap)
    {
        var index = list.IndexOf(elementToSwap);
        if (index == list.Count - 1 || index == -1)
            return;
        var next = list[index + 1];
        list[index] = next;
        list[index + 1] = elementToSwap;
    }
}