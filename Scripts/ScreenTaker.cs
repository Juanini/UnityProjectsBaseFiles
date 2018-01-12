using UnityEngine;
using System.Collections;


/// <summary>
/// ScreenTaker is a simple class that allows to get a picture from a camera as a Texture2D.
/// </summary>
public class ScreenTaker : MonoBehaviour {

	public static ScreenTaker instance;

	void Awake()
	{
		if(instance == null)
			instance = this;
	}

	/// <summary>
	/// Get a screenshot from the whole screen as a Texture2D.
	/// </summary>
	/// <param name="onShotComplete">The callback where the method returns the picture taken.</param>
	/// <param name="format">The image format, default is RGB24.</param>
	public void CaptureScreen(System.Action<Texture2D> onShotComplete, TextureFormat format = TextureFormat.RGB24)
	{
		StartCoroutine(ScreenCapture( onShotComplete,format ));
	}

	/// <summary>
	/// Using a RectTransform you can get a picture from an area of the screen.
	/// </summary>
	/// <param name="screenArea">The area where the image will be taken from.</param>
	/// <param name="onShotComplete">The callback where the method returns the picture taken.</param>
	/// <param name="format">The image format, default is RGB24.</param>
	/// <param name="camera">The camera from where the image will be taken, If you want to take it from the main camera just leave the default wich is null.</param>
	public void CaptureRect(RectTransform screenArea, System.Action<Texture2D> onShotComplete, TextureFormat format = TextureFormat.RGB24, Camera camera = null)
	{
		StartCoroutine(RectCapture(screenArea, onShotComplete,Camera.main, format ));
	}

	IEnumerator ScreenCapture(System.Action<Texture2D> onShotComplete, TextureFormat format)
	{

		Texture2D shot = new Texture2D (Screen.width, Screen.height, format, false);

		yield return new WaitForEndOfFrame ();

		shot.ReadPixels (new Rect (0, 0, Screen.width, Screen.height), 0, 0);
		shot.Apply();

		if(onShotComplete != null)
			onShotComplete(shot);

	}

	IEnumerator RectCapture(RectTransform screenArea ,System.Action<Texture2D> onShotComplete, Camera camera, TextureFormat format)
	{
		Vector3[] corners = new Vector3[4];
		screenArea.GetWorldCorners(corners);

		Vector2 screenMin =  RectTransformUtility.WorldToScreenPoint(camera,corners[0]);
		Vector2 screenMax =  RectTransformUtility.WorldToScreenPoint(camera,corners[2]);

		Texture2D shot = new Texture2D (Mathf.RoundToInt(screenMax.x - screenMin.x), Mathf.RoundToInt(screenMax.y - screenMin.y), format, false);

		yield return new WaitForEndOfFrame ();

		shot.ReadPixels (new Rect (screenMin.x, screenMin.y, screenMax.x, screenMax.y), 0, 0);
		shot.Apply();

		if(onShotComplete != null)
			onShotComplete(shot);

	}
}