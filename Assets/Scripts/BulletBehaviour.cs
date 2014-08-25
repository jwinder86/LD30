using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]
[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(ParticleSystem))]
[RequireComponent (typeof(AudioSource))]
public class BulletBehaviour : MonoBehaviour {

	public float lifetime;

	public ParticleSystem popParticles;

	public AudioClip colorHit;

	private bool alive;
	private GameColor color;
	private float colorAmount;

	// Use this for initialization
	void Start () {
		StartCoroutine(TimerRoutine());
		alive = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision) {
		ReceiveColorBehaviour other = collision.gameObject.GetComponent<ReceiveColorBehaviour>();
		if (other != null) {
			other.ReceiveColor(color, colorAmount);
		}

		audio.PlayOneShot(colorHit);
		DestroySelf ();
	}

	public void SetColor(GameColor color, float amount) {
		this.color = color;
		this.colorAmount = amount;
		renderer.material.color = color.GemColor();

		GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor", color.GemColor());
		popParticles.GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor", color.GemColor());
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
		particleSystem.enableEmission = false;

		popParticles.Play();

		yield return new WaitForSeconds(2f);
		Destroy(gameObject);
	}
}
