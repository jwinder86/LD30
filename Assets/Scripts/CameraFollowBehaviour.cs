using UnityEngine;
using System.Collections;

public class CameraFollowBehaviour : MonoBehaviour {

	private static readonly float X_COEF = 5f;
	private static readonly float Y_COEF = 7f;

	public Transform toFollow;

	public float shakeMagnitude;
	public float shakeSpeed;
	
	private float shakeTimer;

	private Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = transform.position - toFollow.position;
	}
	
	// Update is called once per frame
	void Update() {
		if (shakeTimer > 0f) {
			shakeTimer -= Time.deltaTime;
		}
	}

	void LateUpdate () {

		Vector3 newPosition = toFollow.position + offset;
		
		if (shakeTimer > 0f) {
			newPosition += shakeMagnitude * shakeTimer * new Vector3(Mathf.Sin(Time.time * shakeSpeed * X_COEF), Mathf.Cos(Time.time * shakeSpeed * Y_COEF), 0f);
		}
		
		transform.position = newPosition;
	}

	public void ShakeTime(float time) {
		Debug.Log("Shaking for: " + time);
		shakeTimer = time;
	}
}
