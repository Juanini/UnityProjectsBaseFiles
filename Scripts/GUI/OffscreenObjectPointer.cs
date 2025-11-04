using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffscreenObjectPointer : MonoBehaviour
{
    private Transform target; // El objetivo a seguir
    public RectTransform pointer; // El RectTransform del pointer
    public RectTransform arrow; // El RectTransform de la flecha, hijo del pointer
    public RectTransform canvasRectTransform; // El RectTransform del canvas
    public Camera mainCamera; // La cámara principal que está en uso

    public Button button;

    private void Start()
    {
        mainCamera = Camera.main;
        button.onClick.AddListener(OnClick);
    }

    private bool isActive = false;
    public void SetActive(bool _active)
    {
        isActive = _active;
        pointer.gameObject.SetActive(_active);
    }

    private async void OnClick()
    {
        Game.BlockInput();
        await CamManager.Ins.CenterCameraToObject(target, 0.7f);
        Game.ReleaseInput();
    }

    void Update()
    {
        if (!isActive){ return; }

        if (TutorialManager.Ins.IsActive)
        {
            pointer.gameObject.SetActive(false);
            return;
        }
        
        if(target == null) { return; }
        
        if (IsTargetOnScreen())
        {
            pointer.gameObject.SetActive(false);
        }
        else
        {
            pointer.gameObject.SetActive(true);
            UpdatePointerPositionAndRotation();
        }
    }

    bool IsTargetOnScreen()
    {
        Vector3 worldPosition = target.position;
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(worldPosition);
        
        if (screenPoint.z < 0)
        {
            return false; // Si el objeto está detrás de la cámara, no está en pantalla
        }

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, mainCamera.WorldToScreenPoint(worldPosition), mainCamera, out localPoint);

        return canvasRectTransform.rect.Contains(localPoint);
    }

    void UpdatePointerPositionAndRotation()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);

        if (screenPos.z < 0)
        {
            screenPos *= -1;
        }

        Vector2 canvasPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPos, mainCamera, out canvasPos);

        Vector2 clampedPosition = canvasPos;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -canvasRectTransform.rect.width / 2 + pointer.rect.width / 2, canvasRectTransform.rect.width / 2 - pointer.rect.width / 2);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -canvasRectTransform.rect.height / 2 + pointer.rect.height / 2, canvasRectTransform.rect.height / 2 - pointer.rect.height / 2);

        pointer.localPosition = clampedPosition;

        Vector2 direction = target.position - mainCamera.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // pointer.localRotation = Quaternion.Euler(0, 0, angle);
        arrow.localRotation = Quaternion.Euler(0, 0, angle);
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
        SetActive(true);
    }
}