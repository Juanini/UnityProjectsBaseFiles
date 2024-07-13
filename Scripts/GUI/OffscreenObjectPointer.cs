using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffscreenObjectPointer : MonoBehaviour
{
    public Transform target; // El objetivo
    public Image pointer; // La imagen del puntero (círculo)
    public Image arrow; // La imagen de la flecha

    private Camera mainCamera;
    private RectTransform pointerRectTransform;
    private RectTransform arrowRectTransform;

    void Start()
    {
        mainCamera = Camera.main;
        pointerRectTransform = pointer.GetComponent<RectTransform>();
        arrowRectTransform = arrow.GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);

        bool isOffScreen = screenPos.x <= 0 || screenPos.x >= Screen.width || screenPos.y <= 0 || screenPos.y >= Screen.height;

        pointer.gameObject.SetActive(isOffScreen);
        arrow.gameObject.SetActive(isOffScreen);

        if (isOffScreen)
        {
            Vector3 cappedScreenPos = screenPos;

            cappedScreenPos.x = Mathf.Clamp(cappedScreenPos.x, 0, Screen.width);
            cappedScreenPos.y = Mathf.Clamp(cappedScreenPos.y, 0, Screen.height);

            pointerRectTransform.position = cappedScreenPos;

            Vector3 direction = target.position - mainCamera.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrowRectTransform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}