using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class MatchPopupLayerRect : MonoBehaviour
{
    private RectTransform rectTransform;
    private RectTransform target;
    private readonly Vector3[] corners = new Vector3[4];

    private void Awake()
    {
        rectTransform = (RectTransform)transform;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            if (UI.Ins == null || UI.Ins.uiManager == null) return;
            target = UI.Ins.uiManager.PopUpLayer;
            if (target == null) return;
        }

        var parent = rectTransform.parent as RectTransform;
        if (parent == null) return;

        target.GetWorldCorners(corners);
        Vector2 min = parent.InverseTransformPoint(corners[0]);
        Vector2 max = parent.InverseTransformPoint(corners[2]);

        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta = max - min;
        rectTransform.position = (corners[0] + corners[2]) * 0.5f;
    }
}
