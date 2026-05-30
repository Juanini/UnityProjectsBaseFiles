using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffscreenObjectPointer : MonoBehaviour
{
    private Transform target;
    public RectTransform pointer;
    public RectTransform arrow;
    public RectTransform canvasRectTransform;
    public Camera mainCamera;

    public Button button;

    [Header("Pointer Settings")]
    public Vector2 pointerOffset = Vector2.zero; // <--- NEW

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
            return false;
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

        float halfW = canvasRectTransform.rect.width  / 2;
        float halfH = canvasRectTransform.rect.height / 2;
        float halfPW = pointer.rect.width  / 2;
        float halfPH = pointer.rect.height / 2;

        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -halfW + halfPW, halfW - halfPW);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -halfH + halfPH, halfH - halfPH);

        // Offset pushes inward: sign matches the direction toward screen center
        Vector2 inwardSign = new Vector2(
            clampedPosition.x >= 0 ? -1f : 1f,
            clampedPosition.y >= 0 ? -1f : 1f
        );

        pointer.localPosition = clampedPosition + pointerOffset * inwardSign;

        Vector2 direction = target.position - mainCamera.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.localRotation = Quaternion.Euler(0, 0, angle);
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
        SetActive(true);
    }
}