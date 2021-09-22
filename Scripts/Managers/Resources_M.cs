using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resources_M : MonoBehaviour
{
    public static Resources_M Ins;

    public TextMeshProUGUI logText;
    public TextMeshProUGUI mineralText;

    void Awake()
    {
        Ins = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int logAmount = 0;

    public void AddLog()
    {
        logAmount += 25;
        logText.text = logAmount.ToString();
    }

    int mineralAmount = 0;

    public void AddMineral()
    {
        mineralAmount += 25;
        mineralText.text = mineralAmount.ToString();
    }
}
