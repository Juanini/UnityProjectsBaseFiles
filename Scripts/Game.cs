using System;
using System.Globalization;
using GameEventSystem;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    private static bool isInputBlocked = false;
    public static bool IsOnline { get; set; }
    public static bool IsInputBlocked => isInputBlocked;

    private void Start()
    {
        InvokeRepeating(nameof(CheckForServerConnection),10, 3);
    }

    public static void BlockInput()
    {
        Trace.Log("GAME - BLOCK INPUT");
        isInputBlocked = true;
        GameEventManager.TriggerEvent(GameEvents.BLOCK_INPUT);
    }
    
    public static void ReleaseInput()
    {
        Trace.Log("GAME - RELEASE INPUT");
        isInputBlocked = false;
        GameEventManager.TriggerEvent(GameEvents.RELEASE_INPUT);
    }

    public static string GetDateAndTimeNow()
    {
        DateTime now = DateTime.Now;
        return now.ToString("yyyy-MM-dd HH:mm:ss");
    }
    
    public static bool IsValidDate(string _date)
    {
        DateTime fecha;
        string format = "yyyy-MM-dd HH:mm:ss";
        CultureInfo culture = CultureInfo.InvariantCulture;
        bool isValidDate = DateTime.TryParseExact(_date, format, culture, DateTimeStyles.None, out fecha);
        return isValidDate;
    }

    public static double GetSecondsDifferenceFromDateToNow(string _date)
    {
        DateTime date1;
        if (!DateTime.TryParse(_date, out date1))
        {
            return 0;
        }

        DateTime date2 = DateTime.Parse(GetDateAndTimeNow());

        TimeSpan difference = date2.Subtract(date1);
        return difference.TotalSeconds;
    }
    
    public static double GetSecondsDifferenceBetweenDates(string date1, string date2)
    {
        DateTime startDate;
        DateTime endDate;
        string format = "yyyy-MM-dd HH:mm:ss";
        CultureInfo culture = CultureInfo.InvariantCulture;

        if (DateTime.TryParseExact(date1, format, culture, DateTimeStyles.None, out startDate) &&
            DateTime.TryParseExact(date2, format, culture, DateTimeStyles.None, out endDate))
        {
            TimeSpan difference = endDate.Subtract(startDate);
            return difference.TotalSeconds;
        }
        else
        {
            Trace.Log("Invalid date format.");
            return 0;
        }
    }
    
    public static string GetFutureDateFromSeconds(string startDate, int secondsToAdd)
    {
        DateTime date;
        string format = "yyyy-MM-dd HH:mm:ss";
        CultureInfo culture = CultureInfo.InvariantCulture;

        if (DateTime.TryParseExact(startDate, format, culture, DateTimeStyles.None, out date))
        {
            date = date.AddSeconds(secondsToAdd);
            return date.ToString(format);
        }
        else
        {
            Trace.Log("Invalid start date format.");
            return string.Empty;
        }
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

    private bool isCheckingConnection = false;
    private bool previousConnectionStatus = false;
    private async void CheckForServerConnection()
    {
        if (!isCheckingConnection)
        {
            isCheckingConnection = true;
            previousConnectionStatus = IsOnline;
            IsOnline = await PlayfabServerAPI.Ins.CanReachServer();
            isCheckingConnection = false;

            //If player suddenly switched from Offline to Online, attempt to login to Playfab.
            if(!previousConnectionStatus && IsOnline)
            {
                PlayFabLogin.Ins.Init();
            }
        }
    }

}