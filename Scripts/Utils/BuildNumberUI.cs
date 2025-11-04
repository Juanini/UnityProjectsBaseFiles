using UnityEngine;

public class BuildNumberUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI buildNumberText;

    void Start()
    {
        string buildNumber = GetiOSBuildNumber();
        buildNumberText.text = $"Build # {buildNumber}";
    }

    string GetiOSBuildNumber()
    {
#if UNITY_IOS && !UNITY_EDITOR
        return GetCFBundleVersion();
#else
        return Application.version; // fallback to Version in Editor or other platforms
#endif
    }

#if UNITY_IOS && !UNITY_EDITOR
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern string _GetCFBundleVersion();

    private string GetCFBundleVersion()
    {
        return _GetCFBundleVersion();
    }
#endif
}