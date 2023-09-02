using System.Collections;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Ins;
    
    public Camera cam;
    
    [BoxGroup("ProCamera2D")] public ProCamera2D proCamera2D;
    [BoxGroup("ProCamera2D")] public ProCamera2DPanAndZoom panAndZoom;
    [BoxGroup("ProCamera2D")] public ProCamera2DNumericBoundaries numericBoundaries;
    [BoxGroup("ProCamera2D")] public ProCamera2DContentFitter contentFitter;
    
    void Awake()
    {
        Ins = this;
    }

    public async UniTask DoZoom(float _zoomValue, float _time)
    {
        await cam
            .DOOrthoSize(_zoomValue, _time)
            .SetEase(Ease.InOutExpo)
            .AsyncWaitForCompletion();
    }
    
    // * =====================================================================================================================================
    // * 

    #region ProCamera2D

    public void DisableProCamera()
    {
        proCamera2D.enabled = false;
    }
    
    public void EnableProCamera()
    {
        proCamera2D.enabled = true;
    }
    
    public void DisablePanAndZoomScript()
    {
        panAndZoom.enabled = false;
    }
    
    public void EnablePanAndZoomScript()
    {
        panAndZoom.enabled = true;
    }
    
    public void DisableNumericBoundariesScript()
    {
        numericBoundaries.enabled = false;
    }
    
    public void EnableNumericBoundariesScript()
    {
        numericBoundaries.enabled = true;
    }

    #endregion
}
