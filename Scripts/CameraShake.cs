using UnityEngine;

public class CameraShake : MonoBehaviour {

	public static CameraShake Ins;
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;
	// How long the object should shake for.
	public float shake = 2f;
	private float shakeTime = 0f;
	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.4f;
	public float decreaseFactor = 1.0f;

	public bool enableShake = false;

	Vector3 originalPos;

	void Awake() 
	{
		Ins = this;

		if (camTransform == null) 
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}

	void OnEnable() 
	{
		setCamPos();
	}

	void Start() 
	{
		shakeTime = shake;
	}

	void Update() 
	{
		if (!enableShake) return;

		if (shakeTime > 0) 
		{
			camTransform.localPosition = 
			originalPos + Random.insideUnitSphere * shakeAmount;

			shakeTime -= Time.deltaTime * decreaseFactor;
		}
		else 
		{
			camTransform.localPosition = originalPos;
			enableShake = false;

			shakeTime = shake;
		}
	}

	void OnDestroy() 
	{
		Ins = null;
	}

	public void start2Shake() 
	{
		shakeTime = shake;
		enableShake = true;
	}

    public void start2Shake(float _time)
    {
		shakeTime = _time;
        enableShake = true;
    }

	public void setCamPos()
	{
		originalPos = camTransform.localPosition;
	}
}
