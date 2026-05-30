using System.Collections.Generic;
using UnityEngine;

public static class ArrayExtensions
{
    private static readonly System.Random _random = new System.Random();

    public static T GetRandomElement<T>(this T[] array)
    {
        if (array == null || array.Length == 0)
        {
            Trace.LogError("Array cannot be null or empty");
        }
        
        int randomIndex = _random.Next(array.Length);
        return array[randomIndex];
    }
    
    public static T GetRandomElement<T>(this List<T> list)
    {
        if (list == null || list.Count == 0)
        {
            Trace.LogError("List null or empty");
            return default;
        }
        
        return list[Random.Range(0, list.Count)];
    }
    
    public static void Shuffle<T>(this List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1); // UnityEngine.Random, max is exclusive
            (list[i], list[j]) = (list[j], list[i]); // swap using tuple deconstruction
        }
    }
}