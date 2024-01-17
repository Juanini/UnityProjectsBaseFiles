using System.Diagnostics;
using UnityEngine;

public class Trace : MonoBehaviour {

	[Conditional("USE_LOGS")]
    public static void Log(string format, params object[] arguments)
    {
        UnityEngine.Debug.Log(string.Format(format, arguments));
    }

    [Conditional("USE_LOGS")]
    public static void LogError(string format, params object[] arguments)
    {
        UnityEngine.Debug.LogError(string.Format(format, arguments));
    }
}
