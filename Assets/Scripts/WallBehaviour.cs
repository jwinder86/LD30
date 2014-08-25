using UnityEngine;
using System.Collections;

public class WallBehaviour : MonoBehaviour {

	private bool hit = true;

	public Color color;
	public float flashTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (hit) {
			hit = false;
			StopAllCoroutines();
			StartCoroutine(ShowBehaviour());
		}
	}

	void OnCollisionEnter(Collision collision) {
		ShipBehaviour ship = collision.gameObject.GetComponent<ShipBehaviour>();
		if (ship != null) {
			hit = true;
		}
	}

	private IEnumerator ShowBehaviour() {
		Color endColor = new Color(color.r, color.g, color.b, 0f);
		renderer.enabled = true;

		for (float timer = 0f; timer < flashTime; timer += Time.deltaTime) {
			renderer.material.color = Color.Lerp(color, endColor, timer / flashTime);
			yield return null;
		}

		renderer.enabled = false;
	}
}
