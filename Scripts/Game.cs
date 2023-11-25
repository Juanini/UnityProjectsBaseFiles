using System;
using System.Collections;
using System.Collections.Generic;
using GameEventSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    public static void BlockInput()
    {
        GameEventManager.TriggerEvent(GameEvents.BLOCK_INPUT);
        // EventSystem.current.enabled = false;
    }
    
    public static void ReleaseInput()
    {
        GameEventManager.TriggerEvent(GameEvents.RELEASE_INPUT);
        // EventSystem.current.enabled = true;
    }

    public static string GetDateAndTime()
    {
        DateTime now = DateTime.Now;
        return now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public static double GetSecondsDifference(string _date)
    {
        DateTime date1 = DateTime.Parse(_date);
        DateTime date2 = DateTime.Parse(GetDateAndTime());
        
        TimeSpan difference = date2.Subtract(date1);
        return difference.TotalSeconds;
    }

    public static T GetRandomElementFromArray<T>(T[] array)
    {
        if (array.Length == 0)
        {
            Trace.Log("Empty Array");
            return default(T);
        }

        int randomIndex = Random.Range(0, array.Length);
        return array[randomIndex];
    }
    
    public static string IntToStringWithCommas(int num)
    {
        return $"{num:N0}";
    }

    public static void SetFacingDirection(Transform _transform, Direction _direction)
    {
        var scale = _transform.localScale;
        
        var scaleValue = _direction switch
        {
            Direction.RIGHT => 1,
            Direction.LEFT => -1,
            _ => 1
        };

        scale.x = scaleValue;
        _transform.localScale = scale;
    }
}