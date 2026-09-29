using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using EnhancedUI.EnhancedScroller;

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
    private DG.Tweening.Tween moveTween;
    private EnhancedScroller enhancedScroller;
    private float lastT = -1f;

    private void Awake()
    {
        imageRect = handle ? handle.rectTransform : null;
        imageParent = imageRect ? imageRect.parent as RectTransform : null;
        enhancedScroller = scrollRect ? scrollRect.GetComponent<EnhancedScroller>() : null;
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
        if (!imageRect || !imageParent || !topPosition || !bottomPosition) return;

        float t = GetHandleT();
        bool wrapped = lastT >= 0f && Mathf.Abs(t - lastT) > 0.5f;
        lastT = t;

        Vector2 startLocal = WorldToLocalInParent(topPosition.transform.position);
        Vector2 endLocal   = WorldToLocalInParent(bottomPosition.transform.position);
        Vector2 targetLocal = Vector2.Lerp(startLocal, endLocal, t);

        if (moveDuration > 0f && !wrapped && !isDragging)
        {
            moveTween?.Kill();
            moveTween = imageRect.DOAnchorPos(targetLocal, moveDuration).SetEase(moveEase);
        }
        else if (!isDragging)
        {
            moveTween?.Kill();
            imageRect.anchoredPosition = targetLocal;
        }
    }

    private bool IsLooping()
    {
        return enhancedScroller && enhancedScroller.Loop && enhancedScroller.Delegate != null
               && enhancedScroller.Delegate.GetNumberOfCells(enhancedScroller) > 0;
    }

    private bool TryGetLoopRange(out float groupStart, out float groupSize)
    {
        groupStart = 0f;
        groupSize = 0f;
        if (!IsLooping()) return false;

        int count = enhancedScroller.Delegate.GetNumberOfCells(enhancedScroller);
        groupStart = enhancedScroller.GetScrollPositionForCellViewIndex(count, EnhancedScroller.CellViewPositionEnum.Before);
        groupSize = enhancedScroller.GetScrollPositionForCellViewIndex(count * 2, EnhancedScroller.CellViewPositionEnum.Before) - groupStart;
        return groupSize > 0f;
    }

    private float GetHandleT()
    {
        if (TryGetLoopRange(out float groupStart, out float groupSize))
        {
            float loopT = Mathf.Repeat(enhancedScroller.ScrollPosition - groupStart, groupSize) / groupSize;
            return invert ? 1f - loopT : loopT;
        }

        float v = Mathf.Clamp01(GetVerticalValue());
        return invert ? v : (1f - v);
    }

    private void SetFromHandleT(float t)
    {
        if (TryGetLoopRange(out float groupStart, out float groupSize))
        {
            float loopT = invert ? 1f - t : t;
            enhancedScroller.ScrollPosition = groupStart + Mathf.Min(loopT, 0.999f) * groupSize;
            return;
        }

        scrollRect.verticalNormalizedPosition = invert ? t : (1f - t);
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
        if (scrollRect) scrollRect.StopMovement();

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

        SetFromHandleT(t);
    }

    public void OnEndDrag(PointerEventData _eventData)
    {
        isDragging = false;
        // Optional: could ease to nearest step here if you add stepping.
    }

    #endregion
}
