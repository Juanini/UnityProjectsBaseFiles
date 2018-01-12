using System;
using System.Diagnostics;
using UnityEngine;

public class Trace : MonoBehaviour {

	[Conditional("USE_LOGS")]
    public static void Log(string format, params object[] arguments)
    {
        UnityEngine.Debug.Log(string.Format(format, arguments));
    }
}
