using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Sirenix.OdinInspector;
using TMPro;

public class ButtonCustom : MonoBehaviour
{
    [BoxGroup("Elements")] public Text buttonTextLabel;

    [BoxGroup("Properties")] public string buttonTextStr;

    public Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        buttonTextLabel.text = buttonTextStr;
    }

    public void SetAction(UnityAction action)
    {
        Trace.Log("Button Custom - Action: ");
        button.onClick.AddListener(action);
    }

    public void SetText(string text)
    {
        buttonTextStr = text;
        buttonTextLabel.text = buttonTextStr;
    }
}
