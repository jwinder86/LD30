using UnityEngine;
using System.Collections;

public class GunBehaviour : MonoBehaviour {

	public BulletBehaviour bulletPrefab;
	public float fireDelay;
	public float bulletSpeed = 20f;

	private bool firing;

	// Use this for initialization
	void Start () {
		firing = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Fire1") && !firing) {
			BulletBehaviour bullet = (BulletBehaviour) Instantiate(bulletPrefab, transform.position + transform.up * 3f, Quaternion.identity);
			bullet.rigidbody.velocity = transform.up * bulletSpeed;
			StartCoroutine(FiringRoutine());
		}
	}

	private IEnumerator FiringRoutine() {
		firing = true;
		yield return new WaitForSeconds(fireDelay);
		firing = false;
	}
}
