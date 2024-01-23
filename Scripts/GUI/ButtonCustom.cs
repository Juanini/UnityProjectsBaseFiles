using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Sirenix.OdinInspector;
using TMPro;

public class ButtonCustom : MonoBehaviour
{
    [BoxGroup("Elements")] public TextMeshProUGUI buttonTextLabel;

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
        button.onClick.AddListener(ButtonSound);
    }

    public void SetText(string text)
    {
        buttonTextStr = text;
        buttonTextLabel.text = buttonTextStr;
    }

    public void SetInteractable(bool enabled)
    {
        button.interactable = enabled;
    }

    public void ButtonSound()
    {
        // SoundManager.Ins.playGUISound();
    }
}
