using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class TabUI : MonoBehaviour
{
    public Button button;
    public Image image;
    public Sprite onSprite;
    public Sprite offSprite;
    
    [BoxGroup("CHANGES")] public RectTransform tabImage;
    [BoxGroup("CHANGES")] public RectTransform tabNormalSizeDelta; 
    [BoxGroup("CHANGES")] public RectTransform tabSelectedSizeDelta; 
    
    public void SetState(bool _isActive)
    {
        image.sprite = _isActive ? onSprite : offSprite;
    }

    public void SetSizeDeltaNormal()
    {
        tabImage.DOSizeDelta(tabNormalSizeDelta.sizeDelta, 0.32f).SetEase(Ease.OutElastic, default, 0.4f);
    }
    
    public void SetSizeDeltaSelected()
    {
        HapticsManager.Ins.PlaySelection();
        tabImage.DOSizeDelta(tabSelectedSizeDelta.sizeDelta, 0.32f).SetEase(Ease.OutElastic, default, 0.4f);
    }
}