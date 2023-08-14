using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
public class CameraManager : MonoBehaviour
{
    public static CameraManager Ins;
    
    public Camera cam;
    
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
}
