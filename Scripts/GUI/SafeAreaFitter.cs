using UnityEngine;

public class SafeAreaFitter : MonoBehaviour
{
    public RectTransform container;

    void Start()
    {
        if (container == null)
        {
            Trace.LogError("The RectTransform of the container has not been assigned.");
            return;
        }

        AjustarASafeZone();
        container.gameObject.SetActive(true);
    }

    void AjustarASafeZone()
    {
        Rect safeArea = Screen.safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        container.anchorMin = anchorMin;
        container.anchorMax = anchorMax;
    }
}
