using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class ShipBehaviour : MonoBehaviour {

	public float thrustForce = 1f;
	public float rotSpeed = 180f;
	public float maxSpeed = 5f;

	public float aimingForce = 1f;

	private bool stunned;

	// Use this for initialization
	void Start () {
		stunned = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!stunned) {
			Vector3 mouseDir = Input.mousePosition - new Vector3(Screen.width / 2f, Screen.height / 2f);

			float newAngle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg - 90f;
			newAngle = Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.z, newAngle, rotSpeed * Time.deltaTime);

			transform.rotation = Quaternion.Euler(0f, 0f, newAngle);

			if (Input.GetButton("Fire2")) {
				float dot = Vector3.Dot(rigidbody.velocity, transform.up);

				if (dot < 0f || rigidbody.velocity.magnitude < maxSpeed) {
					rigidbody.AddForce(transform.up * thrustForce, ForceMode.Acceleration);
					Debug.DrawLine(transform.position, transform.position + dot * 5 * transform.up);
				}

				Debug.DrawLine(transform.position, transform.position + rigidbody.velocity * 5);
				Debug.DrawLine(transform.position + transform.up * 5f * maxSpeed + transform.right * 0.1f, transform.position + transform.up * 5f * maxSpeed - transform.right * 0.1f);
				Debug.DrawLine(transform.position, transform.position - transform.right * Vector3.Dot(rigidbody.velocity, transform.right) * aimingForce * 5, Color.green);

				rigidbody.AddForce(-transform.right * Vector3.Dot(rigidbody.velocity, transform.right) * aimingForce, ForceMode.Acceleration);
			}
		}
	}

	public void Stun(Vector3 force) {
		StopAllCoroutines();
		StartCoroutine(StunRoutine());

		rigidbody.AddForce(force, ForceMode.VelocityChange);
	}

	public bool IsStunned() {
		return stunned;
	}

	private IEnumerator StunRoutine() {
		stunned = true;
		yield return new WaitForSeconds(1f);
		rigidbody.angularVelocity = Vector3.zero;
		stunned = false;
	}
}
