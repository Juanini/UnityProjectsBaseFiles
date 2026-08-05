using UnityEngine;

public static class RectTransformExtensions
{
    public static void SetSamePosAndSize(this RectTransform rectTransform, RectTransform source)
    {
        rectTransform.anchorMin = source.anchorMin;
        rectTransform.anchorMax = source.anchorMax;
        rectTransform.pivot = source.pivot;
        rectTransform.sizeDelta = source.sizeDelta;
        rectTransform.anchoredPosition3D = source.anchoredPosition3D;
    }
}
