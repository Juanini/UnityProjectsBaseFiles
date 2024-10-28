using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonWithIconAndText : MonoBehaviour
{
    public Button button;
    public Image image;
    public TextMeshProUGUI text;

    public void SetInteractable(bool _interactable)
    {
        button.interactable = _interactable;
        
        float alpha = _interactable ? 1f : 0.5f;
        
        var imageColor = image.color;
        imageColor.a = alpha;
        image.color = imageColor;
        
        var textColor = text.color;
        textColor.a = alpha;
        text.color = textColor;
    }

    public void SetListener(UnityAction _listener)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(_listener);
    }
}