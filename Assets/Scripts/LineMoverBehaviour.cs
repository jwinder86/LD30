using UnityEngine;
using System.Collections;

[RequireComponent (typeof(TrailRenderer))]
public class LineMoverBehaviour : MonoBehaviour {

	private Vector3 startPos, endPos;
	private float moveTime;

	private float tailTime;
	private bool moving;

	private TrailRenderer trail;

	// Use this for initialization
	void Start () {
		trail = GetComponent<TrailRenderer>();

		tailTime = trail.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (!moving) {
			moving = true;
			StartCoroutine(MoveRoutine());
		}
	}

	public void Configure(Vector3 startPos, Vector3 endPos, Color color, float moveTime) {
		this.startPos = startPos;
		this.endPos = endPos;
		this.moveTime = moveTime;

		trail = GetComponent<TrailRenderer>();
		trail.material.SetColor("_TintColor", color);
	}

	private IEnumerator MoveRoutine() {
		float timer = 0f;

		while (timer < moveTime) {
			transform.position = Vector3.Lerp(startPos, endPos, timer / moveTime);
			timer += Time.deltaTime;
			yield return null;
		}

		yield return new WaitForSeconds(tailTime);

		Destroy(gameObject);
	}
}
