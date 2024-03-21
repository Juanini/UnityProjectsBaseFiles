using System.Collections;
using System.Collections.Generic;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.UI;

public class SOVariableIncrementBttn : MonoBehaviour
{
    public IntVariable myIntVariable;
    public Button myButton;
    public int valueToAdd;

    private void Start()
    {
        if (myButton != null)
        {
            myButton.onClick.AddListener(OnButtonClick);
        }
    }

    private void OnButtonClick()
    {
        myIntVariable.Value += valueToAdd;
    }
}
