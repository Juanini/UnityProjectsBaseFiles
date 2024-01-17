
using System.Collections;
using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour {

	float deltaTime = 0.0f;
	public TextMeshProUGUI fpsLabel;
	private string displayStr = " fps";

	void Awake()
	{
		// GameEventManager.StartListening(GameEvents.FPS_TOGGLE, ToggleFPS);
	}

	void Update()
	{
		if(Time.timeScale <= 0)
		{
			fpsLabel.text = "--" + displayStr;
			return;
		}

		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		//float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		fpsLabel.text = fps.ToString("0") + displayStr;
	}

	private void ToggleFPS(Hashtable _ht)
	{
		fpsLabel.gameObject.SetActive(!fpsLabel.gameObject.activeInHierarchy);
	}
}
