using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(AudioSource))]
[RequireComponent (typeof(ParticleSystem))]
public class ShipBehaviour : MonoBehaviour {

	public Animation shipAnim;
	public Renderer shipModel;

	public float thrustForce = 1f;
	public float rotSpeed = 180f;
	public float maxSpeed = 5f;

	public float aimingForce = 1f;

	public float stunTime = 1f;
	public float invulTime = 2f;

	public float bounceTime = 0.3f;
	public float bounceForce = 10f;

	public AudioClip bumpSound;
	public AudioClip hurtSound;

	public ParticleSystem exhaust;

	private bool stunned;
	private bool invulnerable;

	private ReservoirBehaviour reservoir;

	private CameraFollowBehaviour cam;

	// Use this for initialization
	void Start () {
		stunned = false;

		reservoir = GetComponent<ReservoirBehaviour>();

		cam = (CameraFollowBehaviour) FindObjectOfType(typeof(CameraFollowBehaviour));
	}
	
	// Update is called once per frame
	void Update () {
		if (!stunned) {
			Vector3 mouseDir = Input.mousePosition - new Vector3(Screen.width / 2f, Screen.height / 2f);

			float newAngle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg - 90f;
			newAngle = Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.z, newAngle, rotSpeed * Time.deltaTime);

			transform.rotation = Quaternion.Euler(0f, 0f, newAngle);

			if (Input.GetButton("Fire2")) {
				shipAnim.Play();
				exhaust.enableEmission = true;

				float dot = Vector3.Dot(rigidbody.velocity, transform.up);

				if (dot < 0f || rigidbody.velocity.magnitude < maxSpeed) {
					rigidbody.AddForce(transform.up * thrustForce, ForceMode.Acceleration);
					Debug.DrawLine(transform.position, transform.position + dot * 5 * transform.up);
				}

				Debug.DrawLine(transform.position, transform.position + rigidbody.velocity * 5);
				Debug.DrawLine(transform.position + transform.up * 5f * maxSpeed + transform.right * 0.1f, transform.position + transform.up * 5f * maxSpeed - transform.right * 0.1f);
				Debug.DrawLine(transform.position, transform.position - transform.right * Vector3.Dot(rigidbody.velocity, transform.right) * aimingForce * 5, Color.green);

				rigidbody.AddForce(-transform.right * Vector3.Dot(rigidbody.velocity, transform.right) * aimingForce, ForceMode.Acceleration);
			} else {
				shipAnim.Stop();
				exhaust.enableEmission = false;
			}
		} else {
			shipAnim.Stop();
			exhaust.enableEmission = false;
		}
	}

	void OnCollisionEnter(Collision collision) {
		int layer = collision.gameObject.layer;
		if (!stunned && !invulnerable && !(layer == LayerMask.NameToLayer("Enemy") || layer == LayerMask.NameToLayer("EnemyBullet"))) {
			StopAllCoroutines();
			StartCoroutine(StunRoutine(bounceTime, collision.contacts[0].normal * bounceForce, false));

			audio.PlayOneShot(bumpSound);
			cam.ShakeTime(0.4f);
		}
	}

	public void Stun(Vector3 force) {
		if (!stunned && !invulnerable) {
			StopAllCoroutines();
			StartCoroutine(StunRoutine(stunTime, force, true));

			audio.PlayOneShot(hurtSound);
			cam.ShakeTime(1f);

			float lost = reservoir.GetColorFromTank(3f);
			GameColor hitColor = reservoir.GetCurrentColor();
			ParticleSystemRenderer psr = GetComponent<ParticleSystemRenderer>();
			if (hitColor != null && lost > 0f) {
				psr.material.SetColor("_TintColor", hitColor.GemColor());
			} else {
				psr.material.SetColor("_TintColor", Color.white);
			}
			particleSystem.Play();
		}
	}

	public bool IsStunned() {
		return stunned;
	}

	private IEnumerator StunRoutine(float time, Vector3 force, bool becomeInvul) {
		stunned = true;
		rigidbody.AddForce(force, ForceMode.VelocityChange);

		if (becomeInvul) {
			Color startColor = shipModel.material.color;
			for (float timer = 0f; timer < time; timer += Time.deltaTime) {
				float d = timer / time * 4f;
				shipModel.material.color = Color.Lerp(Color.red, startColor, d - Mathf.Floor(d));
				yield return null;
			}
			shipModel.material.color = startColor;
		} else {
			yield return new WaitForSeconds(time);
		}

		rigidbody.angularVelocity = Vector3.zero;
		stunned = false;

		if (becomeInvul) {
			Debug.Log("invulnerable");
			invulnerable = true;

			Color startColor = shipModel.material.color;
			for (float timer = 0f; timer < invulTime; timer += Time.deltaTime) {
				float d = timer / invulTime * 4f;
				shipModel.material.color = Color.Lerp(Color.red, startColor, d - Mathf.Floor(d));
				yield return null;
			}
			shipModel.material.color = startColor;

			invulnerable = false;
			Debug.Log("vulnerable");
		}
	}
}
