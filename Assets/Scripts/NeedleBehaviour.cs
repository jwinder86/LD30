using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(Rigidbody))]
public class NeedleBehaviour : MonoBehaviour {

	public Transform target;
	public float attackRange = 50f;
	public float turnRate = 360f;
	public float attackAngleDiff = 5f;

	public float attackDelay = 0.5f;
	public float attackSpeed = 20f;
	public float attackTime = 1f;
	public float attackCoolDown = 2f;
	public float attackForce = 10f;

	private bool attacking;

	// Use this for initialization
	void Start () {
		attacking = false;
		rigidbody.isKinematic = true;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawLine(transform.position, transform.position + (target.position - transform.position).normalized * attackRange, Color.red);
		if ((target.position - transform.position).magnitude <= attackRange && !attacking) {
			float angleError = Mathf.Abs(RotateTowardTarget());
			Debug.Log("Angle diff" + angleError);
			if (angleError < attackAngleDiff) {
				StartCoroutine(AttackRoutine());
			}
		}
	}

	void OnCollisionEnter(Collision collision) {
		ShipBehaviour ship = collision.gameObject.GetComponent<ShipBehaviour>();
		if (ship != null) {
			Vector3 force = (ship.transform.position - transform.position).normalized * attackForce;
			ship.Stun(force);
		}

		StopAllCoroutines();
		StartCoroutine(DelayRoutine());
	}

	private float RotateTowardTarget() {
		Vector3 toTarget = (target.position - transform.position).normalized;
		float angle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg - 90f;

		float newAngle = Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.z, angle, turnRate * Time.deltaTime);

		transform.rotation = Quaternion.Euler(0f, 0f, newAngle);

		return Mathf.DeltaAngle(angle, newAngle);
	}

	private IEnumerator DelayRoutine() {
		attacking = true;
		animation.Play();
		yield return new WaitForSeconds(attackCoolDown);
		attacking = false;
	}

	private IEnumerator AttackRoutine() {
		attacking = true;
		Debug.Log("Starting attack");

		animation.Stop();
		yield return new WaitForSeconds(attackDelay);

		float attackTimer = 0f;
		while (attackTimer < attackTime) {
			rigidbody.position += transform.up * attackSpeed * Time.deltaTime;
			attackTimer += Time.deltaTime;
			yield return null;
		}

		animation.Play();

		float cooldownTimer = 0f;
		while (cooldownTimer < attackCoolDown) {
			RotateTowardTarget();
			cooldownTimer += Time.deltaTime;
			yield return null;
		}

		rigidbody.velocity = Vector3.zero;

		attacking = false;
		Debug.Log("Ending attack");
	}
}
