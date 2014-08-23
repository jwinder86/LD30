using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]
[RequireComponent (typeof(Rigidbody))]
public class BulletBehaviour : MonoBehaviour {

	public float lifetime;

	private bool alive;

	// Use this for initialization
	void Start () {
		StartCoroutine(TimerRoutine());
		alive = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision) {
		DestroySelf ();
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
