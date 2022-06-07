using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public GameObject bar;
    public GameObject startPos;
    public GameObject completePos;
    public TextMeshProUGUI valueText;

    private int maxValue = 100;
    public float value;

    private float incrementValue;

    void Start()
    {
        incrementValue = (completePos.transform.localPosition.x - startPos.transform.localPosition.x) / maxValue;
        Trace.Log(this.name + " - " + "IncrementValue = " + incrementValue);
    }
    
    [Button(ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
    public void SetupBar(int _maxValue)
    {
        DOTween.Kill(bar.transform);
        maxValue = _maxValue;
        incrementValue = (completePos.transform.localPosition.x - startPos.transform.localPosition.x) / maxValue;
        bar.transform.localPosition = startPos.transform.localPosition;
    }
    
    [Button(ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
    public void DoValueBalance( float _value,
        float _time,
        UnityAction _updateCallback = null)
    {
        if(_value > maxValue) { return; }

        DOTween.To(()=> value, x=> value = x, _value, _time)
            .OnUpdate(OnValueUpdate)
            .OnComplete(OnValueComplete)
            .SetEase(Ease.Linear);

        bar.transform.DOLocalMove(new Vector3(bar.transform.localPosition.x + (incrementValue * (_value - value)), 0, 0), _time)
            .SetEase(Ease.InOutQuint);

        updateCall = _updateCallback;
    }
    
    public void ShowText()
    {
        valueText.text = 0 + "/" + maxValue;
    }

    UnityAction updateCall;

    private void OnValueUpdate()
    {
        if(valueText == null) return;
        valueText.text = (int)value + "/" + maxValue;

        if(updateCall != null)
            updateCall.Invoke();
    }

    public void ResetVal()
    {
        value = 0;
        bar.transform.localPosition = startPos.transform.localPosition;
    }

    private void OnValueComplete()
    {   

    }
}
