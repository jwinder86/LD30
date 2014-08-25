using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]
[RequireComponent (typeof(AudioSource))]
public class ColorBlobBehaviour : MonoBehaviour {

	public ParticleSystem particles;
	public GameColor.ColorName colorName;
	public float amount;

	public float refillTime = 10f;

	public AudioClip refill;

	private GameColor color;

	private bool empty;

	// Use this for initialization
	void Start () {
		if (color == null) {
			color = GameColor.FromName(colorName);
		}

		renderer.material.color = color.GemColor();
		ParticleSystemRenderer pr = particles.GetComponent<ParticleSystemRenderer>();
		pr.material.SetColor("_TintColor", color.GemColor());

		StartCoroutine(CreateRoutine());
	}

	public void SetColor(GameColor color) {
		this.color = color;
	}
	
	// Update is called once per frame
	void Update () {
		if (empty) {
			StartCoroutine(RefillRoutine());
		}
	}

	void OnTriggerEnter(Collider otherCollider) {
		ReceiveColorBehaviour other = otherCollider.GetComponent<ReceiveColorBehaviour>();

		if (other != null) {
			other.ReceiveColor(color, amount);
			renderer.enabled = false;
			collider.enabled = false;
			empty = true;
		}
	}

	private IEnumerator RefillRoutine() {
		empty = false;
		yield return new WaitForSeconds(refillTime);

		StartCoroutine(CreateRoutine());
	}

	private IEnumerator CreateRoutine() {
		particles.Emit(100);
		particles.Play();
		
		yield return new WaitForSeconds(0.5f);

		renderer.enabled = true;
		collider.enabled = true;
	}
}
