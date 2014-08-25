using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class EndBehviour : MonoBehaviour {

	private FadeBehaviour fade;
	private bool trigger;

	public AudioClip powerup;
	public AudioClip bigPowerup;

	// Use this for initialization
	void Start () {
		fade = (FadeBehaviour) FindObjectOfType(typeof(FadeBehaviour));
	}
	
	// Update is called once per frame
	void Update () {
		if (trigger) {
			StartCoroutine(EndRoutine());
			trigger = false;
		}
	}

	void OnTriggerEnter(Collider other) {
		ShipBehaviour ship = other.GetComponent<ShipBehaviour>();
		if (ship != null) {
			trigger = true;
			collider.enabled= false;
		}
	}

	private IEnumerator EndRoutine() {
		ShipBehaviour ship = (ShipBehaviour) FindObjectOfType(typeof(ShipBehaviour));
		ship.rigidbody.isKinematic = true;
		Destroy(ship);

		audio.PlayOneShot(powerup);

		for (float timer = 0f; timer < 2f; timer += Time.deltaTime) {
			float newScale = 3f + timer / 2f * 10f;
			transform.localScale = new Vector3(newScale, newScale, newScale);
			yield return null;
		}

		animation.CrossFade("GemRotate", 1f, PlayMode.StopAll);

		audio.PlayOneShot(bigPowerup);

		for (float timer = 0f; timer < 1f; timer += Time.deltaTime) {
			float newScale = 10f - timer / 1f * 4f;
			float yScale = 10f + timer / 1f * 50f;
			transform.localScale = new Vector3(newScale, yScale, newScale);
			yield return null;
		}

		fade.FadeOut();
		yield return new WaitForSeconds(2f);
		Application.LoadLevel("TitleScene");
	}
}
