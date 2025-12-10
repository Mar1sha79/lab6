using System;
using System.Collections.Generic;
using System.Linq;

public static class MySetAnalysis
{
    public static (HashSet<string> inAll, HashSet<string> inSome, HashSet<string> inOne)
        AnalyzeSets(Dictionary<int, HashSet<string>> sets)
    {
        if (sets == null || sets.Count == 0)
            return (new HashSet<string>(), new HashSet<string>(), new HashSet<string>());

        var elementCount = new Dictionary<string, int>();
        var allElements = new HashSet<string>();

        foreach (var set in sets.Values)
        {
            allElements.UnionWith(set);
            foreach (var element in set)
            {
                if (elementCount.ContainsKey(element))
                    elementCount[element]++;
                else
                    elementCount[element] = 1;
            }
        }

        int totalSets = sets.Count;
        var inAll = new HashSet<string>();
        var inSome = new HashSet<string>();
        var inOne = new HashSet<string>();

        foreach (var element in allElements)
        {
            int count = elementCount[element];

            if (count == totalSets)
                inAll.Add(element);
            else if (count == 1)
                inOne.Add(element);
            else
                inSome.Add(element);
        }

        return (inAll, inSome, inOne);
    }
}