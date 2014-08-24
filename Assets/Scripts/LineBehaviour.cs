using UnityEngine;
using System.Collections;

public class LineBehaviour : MonoBehaviour {

	public LineMoverBehaviour moverPrefab;

	public float zPos = 10f;

	private Vector3 start, end;
	private Color color;
	private float moveTime, moveFreq, moveOffset;

	private bool moving;
	private bool alive, shouldDie;

	// Use this for initialization
	void Start () {
		moving = false;
		alive = true;
		shouldDie = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!moving && alive) {
			LineMoverBehaviour mover = (LineMoverBehaviour) Instantiate(moverPrefab, transform.position, Quaternion.identity);
			mover.Configure(start + Random.onUnitSphere * moveOffset, end + Random.onUnitSphere * moveOffset, color, moveTime);
			StartCoroutine(WaitRoutine());

			if (shouldDie) {
				StartCoroutine(DeathRoutine());
			}
		}
	}

	public void Stop() {
		shouldDie = true;
	}

	public void Configure(Vector3 start, Vector3 end, Color color, float moveSpeed, float moveFreq, float moveOffset) {
		this.start = start;
		this.end = end;
		this.color = color;
		this.moveTime = (start - end).magnitude / moveSpeed;
		this.moveFreq = moveFreq;
		this.moveOffset = moveOffset;

		this.start.z = zPos;
		this.end.z = zPos;

		transform.position = start;
	}

	private IEnumerator DeathRoutine() {
		alive = false;
		yield return new WaitForSeconds(moveTime);
		Destroy(gameObject);
	}

	private IEnumerator WaitRoutine() {
		moving = true;
		yield return new WaitForSeconds(moveFreq);
		moving = false;
	}
}
