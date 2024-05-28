using System;

public static class ArrayExtensions
{
    private static readonly System.Random _random = new System.Random();

    public static T GetRandomElement<T>(this T[] array)
    {
        if (array == null || array.Length == 0)
        {
            throw new ArgumentException("Array cannot be null or empty");
        }
        
        int randomIndex = _random.Next(array.Length);
        return array[randomIndex];
    }
}