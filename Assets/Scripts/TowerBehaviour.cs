using UnityEngine;
using System.Collections;

public class TowerBehaviour : HandleColorHitBehaviour {

	public TowerBullet bulletPrefab;
	public float fireDelay = 0.5f;
	public float bulletSpeed = 20f;
	public float attackRange = 50f;
	public float life = 1f;

	private Rigidbody target;
	private bool attacking;
	private bool alive;

	// Use this for initialization
	void Start () {
		attacking = false;
		target = ((ShipBehaviour)FindObjectOfType(typeof(ShipBehaviour))).rigidbody;
		alive = true;
	}
	
	// Update is called once per frame
	void Update () {

		if (alive) {
			if ((target.position - transform.position).magnitude <= attackRange && !attacking) {
				Vector3 toTarget = (target.position + target.velocity * 0.2f - transform.position).normalized;
				float angle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg - 90f;

				TowerBullet bullet = (TowerBullet) Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, angle));
				bullet.rigidbody.velocity = toTarget * bulletSpeed;

				StartCoroutine(AttackRoutine());
			}
			
			if (life <= 0f) {
				StopAllCoroutines();
				StartCoroutine(DeathRoutine());
			}
		}
	}

	public override void HandleColor(GameColor color, float amount) {
		life -= amount;
	}

	private IEnumerator AttackRoutine() {
		attacking = true;

		yield return new WaitForSeconds(fireDelay);
		
		attacking = false;
	}
	
	private IEnumerator DeathRoutine() {
		alive = false;
		yield return new WaitForSeconds(1f);


		Destroy(gameObject);
	}
}
