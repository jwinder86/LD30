using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]
[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof (LineRenderer))]
public class TowerBullet : MonoBehaviour {

	public float lifetime = 5f;
	public float length;
	public float width;
	public float attackForce = 10f;

	private LineRenderer line;
	private bool alive;

	// Use this for initialization
	void Start () {
		line = GetComponent<LineRenderer>();

		StartCoroutine(TimerRoutine());
		alive = true;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		DrawLine();
	}

	void OnCollisionEnter(Collision collision) {
		ShipBehaviour ship = collision.gameObject.GetComponent<ShipBehaviour>();
		if (ship != null) {
			Vector3 force = (ship.transform.position - transform.position).normalized * attackForce;
			ship.Stun(force);
		}
		
		DestroySelf ();
	}

	private void DrawLine() {
		int points = Random.Range(3, 7);

		line.SetVertexCount(points + 1);
		line.SetPosition(points, new Vector3(0f, length / 2f, 0f));

		for (int i = 0; i <= points; i++) {
			line.SetPosition(i, new Vector3(Random.Range(-width / 2f, width / 2f), length * i / points - length / 2f, 0f));
		}
	}

	private void DestroySelf() {
		if (alive) {
			alive = false;
			StartCoroutine(DestroyRoutine());
		}
	}
	
	private IEnumerator TimerRoutine() {
		yield return new WaitForSeconds(lifetime);
		DestroySelf();
	}
	
	private IEnumerator DestroyRoutine() {
		renderer.enabled = false;
		collider.enabled = false;
		rigidbody.isKinematic = true;
		
		yield return new WaitForSeconds(2f);
		Destroy(gameObject);
	}
}
