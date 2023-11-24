using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class TouchDetector : MonoBehaviour, IPointerClickHandler
{
    public delegate void TouchedAction(PointerEventData eventData);
    public event TouchedAction OnTouched;

    private BoxCollider2D boxCollider2D;

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnTouched?.Invoke(eventData);
    }

    public void Enable()
    {
        boxCollider2D.enabled = true;
    }

    public void Disable()
    {
        boxCollider2D.enabled = false;
    }
}