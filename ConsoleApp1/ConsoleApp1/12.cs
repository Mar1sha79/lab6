using System;
using System.Collections.Generic;

public static class MyCollectionOperations
{
    public static int CountDistinctElements(List<int> list)
    {
        if (list == null || list.Count == 0)
            return 0;

        return new HashSet<int>(list).Count;
    }

    public static void ReplaceNeighborsByPosition(LinkedList<int> list, int position, int replacement)
    {
        if (list == null || list.Count < 3)
            return;

        if (position < 0 || position >= list.Count)
            throw new ArgumentException("Позиция должна быть в пределах от 0 до " + (list.Count - 1));

        var currentNode = list.First;
        for (int i = 0; i < position; i++)
        {
            currentNode = currentNode.Next;
        }

        if (currentNode.Previous != null)
        {
            currentNode.Previous.Value = replacement;
        }

        if (currentNode.Next != null)
        {
            currentNode.Next.Value = replacement;
        }
    }
}