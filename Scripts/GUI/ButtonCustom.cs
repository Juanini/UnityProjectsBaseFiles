using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Sirenix.OdinInspector;
using TMPro;

public class ButtonCustom : MonoBehaviour
{
    [BoxGroup("Elements")] public TextMeshProUGUI buttonTextLabel;

    [BoxGroup("Properties")] public string buttonTextStr;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        buttonTextLabel.text = buttonTextStr;
    }

    public void SetAction(UnityAction action)
    {
        button.onClick.AddListener(action);
    }
}
