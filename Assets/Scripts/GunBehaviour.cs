using UnityEngine;
using System.Collections;

[RequireComponent (typeof(ReservoirBehaviour))]
public class GunBehaviour : MonoBehaviour {

	public BulletBehaviour bulletPrefab;
	public float fireDelay;
	public float bulletSpeed = 20f;
	public float chargeRate = 3f;

	private float shotPower;

	private ReservoirBehaviour reservoir;

	// Use this for initialization
	void Start () {
		reservoir = GetComponent<ReservoirBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Fire1")) {
			shotPower += reservoir.GetColorFromTank(Time.deltaTime * chargeRate);
		}

		if (Input.GetButtonUp("Fire1") && shotPower > 0f) {
			float radius = Mathf.Sqrt(shotPower);
			BulletBehaviour bullet = (BulletBehaviour) Instantiate(bulletPrefab, transform.position + transform.up * (2f + radius), Quaternion.identity);
			bullet.SetColor(reservoir.GetCurrentColor(), shotPower);
			bullet.transform.localScale = new Vector3(radius, radius, radius);
			bullet.rigidbody.velocity = transform.up * bulletSpeed;

			shotPower = 0f; 
		}
	}


}
