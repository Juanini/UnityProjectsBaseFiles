using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabUI : MonoBehaviour
{
    public Button button;
    public Image image;
    public Sprite onSprite;
    public Sprite offSprite;

    public void SetState(bool _isActive)
    {
        image.sprite = _isActive ? onSprite : offSprite;
    }
}
