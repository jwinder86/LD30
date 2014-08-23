using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class ShipBehaviour : MonoBehaviour {

	public float thrustForce = 1f;
	public float rotSpeed = 180f;
	public float maxSpeed = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 mouseDir = Input.mousePosition - new Vector3(Screen.width / 2f, Screen.height / 2f);

		float newAngle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg - 90f;
		newAngle = Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.z, newAngle, rotSpeed * Time.deltaTime);

		transform.rotation = Quaternion.Euler(0f, 0f, newAngle);

		if (Input.GetButton("Fire1") && (Vector3.Dot(rigidbody.velocity, transform.up) < 0f || rigidbody.velocity.magnitude < maxSpeed)) {
			float dot = Vector3.Dot(rigidbody.velocity, transform.up);

			Debug.DrawLine(transform.position, transform.position + dot * 5 * transform.up);
			Debug.DrawLine(transform.position, transform.position + rigidbody.velocity * 5);
			Debug.DrawLine(transform.position + transform.up * 5f * maxSpeed + transform.right * 0.1f, transform.position + transform.up * 5f * maxSpeed - transform.right * 0.1f);

			rigidbody.AddForce(transform.up * thrustForce, ForceMode.Acceleration);
		}
	}
}
