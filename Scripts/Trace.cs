using System.Diagnostics;
using UnityEngine;

public class Trace : MonoBehaviour {

	// [Conditional("USE_LOGS")]
    // public static void Log(string format, params object[] arguments)
    // {
    //     UnityEngine.Debug.Log(string.Format(format, arguments));
    // }
    
    [Conditional("USE_LOGS")]
    public static void Log(string text, Object context = null)
    {
        UnityEngine.Debug.Log(text, context);
    }

    [Conditional("USE_LOGS")]
    public static void LogError(string format, params object[] arguments)
    {
        UnityEngine.Debug.LogError(string.Format(format, arguments));
    }
    
    [Conditional("USE_LOGS")]
    public static void LogWarning(string format, params object[] arguments)
    {
        UnityEngine.Debug.LogWarning(string.Format(format, arguments));
    }

    [Conditional("USE_LOGS")]
    public static void LogError(UnityEngine.Object context, string format, params object[] arguments)
    {
        UnityEngine.Debug.LogErrorFormat(context, format, arguments);
    }
}
