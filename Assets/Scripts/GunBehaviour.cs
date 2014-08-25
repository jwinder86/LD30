using UnityEngine;
using System.Collections;

[RequireComponent (typeof(ReservoirBehaviour))]
[RequireComponent (typeof(AudioSource))]
public class GunBehaviour : MonoBehaviour {

	public BulletBehaviour bulletPrefab;
	public float bulletSpeed = 20f;
	public float chargeRate = 3f;

	public AudioClip shootSound;

	private float shotPower;

	private ReservoirBehaviour reservoir;
	private ShipBehaviour ship;

	// Use this for initialization
	void Start () {
		reservoir = GetComponent<ReservoirBehaviour>();
		ship = GetComponent<ShipBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
		if (ship.IsStunned()) {
			shotPower = 0f;
		} else {
			if (Input.GetButton("Fire1")) {
				shotPower += reservoir.GetColorFromTank(Time.deltaTime * chargeRate);
			}

			if (Input.GetButtonUp("Fire1") && shotPower > 0f) {
				audio.PlayOneShot(shootSound);

				float radius = Mathf.Sqrt(shotPower);
				BulletBehaviour bullet = (BulletBehaviour) Instantiate(bulletPrefab, transform.position + transform.up * (2f + radius), Quaternion.identity);
				bullet.SetColor(reservoir.GetCurrentColor(), shotPower);
				bullet.transform.localScale = new Vector3(radius, radius, radius);
				bullet.rigidbody.velocity = transform.up * bulletSpeed;

				shotPower = 0f; 
			}
		}
	}


}
