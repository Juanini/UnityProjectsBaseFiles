using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadePanel : MonoBehaviour
{
    public static FadePanel Ins;

    public Image panel;
    void Awake() => Ins = this;

    private UnityAction fadeItCallback, fadeOutCallback;
    
    // * =====================================================================================================================================
    // * IN 
    
    [Button(ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
    public void FadeIn(float _time = 1, UnityAction _callback = null)
    {
        fadeItCallback = _callback;
        panel.DOFade(1, _time)
            .SetEase(Ease.Linear)
            .OnComplete(OnFadeInComplete);
    }

    private void OnFadeInComplete() => fadeItCallback?.Invoke();

    // * =====================================================================================================================================
    // * OUT
    
    [Button(ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
    public void FadeOut(float time = 1, UnityAction _callback = null)
    {
        panel.DOFade(0, time)
            .SetEase(Ease.Linear)
            .OnComplete(OnFadeOutComplete);
    }

    private void OnFadeOutComplete() => fadeOutCallback?.Invoke();
}
