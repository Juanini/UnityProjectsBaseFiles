using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]  // Se cambia a Collider2D para generalizar
public class TouchDetector : MonoBehaviour, IPointerClickHandler
{
    public delegate void TouchedAction(PointerEventData eventData);
    public event TouchedAction OnTouched;

    private Collider2D collider2D;  // Cambio de BoxCollider2D a Collider2D

    private void Start()
    {
        collider2D = GetComponent<Collider2D>();  // Obtiene cualquier tipo de Collider2D
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnTouched?.Invoke(eventData);
    }

    public void Enable()
    {
        collider2D.enabled = true;
    }

    public void Disable()
    {
        collider2D.enabled = false;
    }
}