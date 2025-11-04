using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

public class ScrollBarCustom : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    // Public first (camelCase)
    public ScrollRect scrollRect;

    [BoxGroup("MAIN")] public Image handle;
    [BoxGroup("MAIN")] public GameObject topPosition;   // typically the TOP marker
    [BoxGroup("MAIN")] public GameObject bottomPosition;     // typically the BOTTOM marker

    [Header("Behavior")]
    public bool invert = false;        // if true: v=0->start, v=1->end ; if false: v=1->start, v=0->end
    public float moveDuration = 0f;    // 0 = snap, >0 = smooth with DOTween
    public Ease moveEase = Ease.Linear;

    // Privates afterward
    private RectTransform imageRect;
    private RectTransform imageParent;
    private Tween moveTween;

    private void Awake()
    {
        imageRect = handle ? handle.rectTransform : null;
        imageParent = imageRect ? imageRect.parent as RectTransform : null;
    }

    private void OnEnable()
    {
        scrollRect.onValueChanged.AddListener(OnScrollChanged);
        OnScrollChanged(scrollRect.normalizedPosition); // init once
    }

    private void OnDisable()
    {
        scrollRect.onValueChanged.RemoveListener(OnScrollChanged);
        moveTween?.Kill();
    }

    private void OnScrollChanged(Vector2 _normalizedPos)
    {
        float v = Mathf.Clamp01(GetVerticalValue()); // 0 = bottom, 1 = top
        // Trace.Log(this.name + " - " + "VERTICAL POS = " + v.ToString("0.000"));

        if (!imageRect || !imageParent || !topPosition || !bottomPosition) return;

        // Map normalized value to [start,end]
        // Default (invert=false): v=1 -> start, v=0 -> end
        float t = invert ? v : (1f - v);

        Vector2 startLocal = WorldToLocalInParent(topPosition.transform.position);
        Vector2 endLocal   = WorldToLocalInParent(bottomPosition.transform.position);
        Vector2 targetLocal = Vector2.Lerp(startLocal, endLocal, t);

        if (moveDuration > 0f)
        {
            moveTween?.Kill();
            moveTween = imageRect.DOAnchorPos(targetLocal, moveDuration).SetEase(moveEase);
        }
        else
        {
            imageRect.anchoredPosition = targetLocal;
        }
    }

    private Vector2 WorldToLocalInParent(Vector3 _worldPos)
    {
        return imageParent ? (Vector2)imageParent.InverseTransformPoint(_worldPos) : (Vector2)_worldPos;
    }

    public float GetVerticalValue()   => scrollRect.verticalNormalizedPosition;
    public float GetHorizontalValue() => scrollRect.horizontalNormalizedPosition;
    
    // ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    // * 
    
    #region Drag

    private bool isDragging;
    private Vector2 dragOffsetInParent;  
    
    public void OnPointerDown(PointerEventData _eventData)
    {
        // Allow clicking the handle without moving immediately; we only prep offset here.
        if (!imageRect || !imageParent) return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                imageParent, _eventData.position, _eventData.pressEventCamera, out var pointerLocal))
        {
            dragOffsetInParent = imageRect.anchoredPosition - pointerLocal;
        }
    }

    public void OnBeginDrag(PointerEventData _eventData)
    {
        if (!imageRect || !imageParent) return;
        isDragging = true;
        moveTween?.Kill(); // stop any smooth tween while user drags

        // Recompute offset in case drag started without prior pointer down on the handle
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                imageParent, _eventData.position, _eventData.pressEventCamera, out var pointerLocal))
        {
            dragOffsetInParent = imageRect.anchoredPosition - pointerLocal;
        }
    }

    public void OnDrag(PointerEventData _eventData)
    {
        if (!imageRect || !imageParent || !topPosition || !bottomPosition || scrollRect == null) return;

        // Convert pointer pos to parent space
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                imageParent, _eventData.position, _eventData.pressEventCamera, out var pointerLocal))
            return;

        // Desired pos (keep the initial offset so we don't "snap" under the finger)
        Vector2 desired = pointerLocal + dragOffsetInParent;

        // Get vertical rail (from top to bottom) in parent space
        Vector2 startLocal = WorldToLocalInParent(topPosition.transform.position);
        Vector2 endLocal   = WorldToLocalInParent(bottomPosition.transform.position);

        // Clamp to the vertical segment (assumes a vertical scrollbar; x is fixed to rail x)
        float railX = Mathf.Lerp(startLocal.x, endLocal.x, 0.5f); // if not perfectly vertical, take mid X
        float minY = Mathf.Min(startLocal.y, endLocal.y);
        float maxY = Mathf.Max(startLocal.y, endLocal.y);

        Vector2 clamped = new Vector2(railX, Mathf.Clamp(desired.y, minY, maxY));
        imageRect.anchoredPosition = clamped;

        // Compute t along [top..bottom] => 0 at top, 1 at bottom
        float t = Mathf.InverseLerp(startLocal.y, endLocal.y, clamped.y);

        // Map to ScrollRect verticalNormalizedPosition:
        // Unity: v=1 top, v=0 bottom
        float v = invert ? t : (1f - t);

        scrollRect.verticalNormalizedPosition = v;
    }

    public void OnEndDrag(PointerEventData _eventData)
    {
        isDragging = false;
        // Optional: could ease to nearest step here if you add stepping.
    }

    #endregion
}
