using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class CrystalBehaviour : HandleColorHitBehaviour {

	public ColorBlobBehaviour blobPrefab;
	public float colorRequired;

	public bool startActive;
	public GameColor.ColorName startColorName;

	public float spawnHeight = 10f;

	public Renderer[] otherGems;

	public AudioClip colorGot;
	public AudioClip colorFilled;

	private bool active;
	private float colorAmount;
	private GameColor color;

	// Use this for initialization
	void Start () {
		if (startActive) {
			GameColor startColor = GameColor.FromName(startColorName);
			StartCoroutine(ActivateRoutine(startColor));
		}

		colorAmount = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (!active && colorAmount >= colorRequired && color != null) {
			StopAllCoroutines();
			StartCoroutine(ActivateRoutine(color));
		}
	}

	public override void HandleColor(GameColor color, float amount) {
		if (!active) {
			audio.PlayOneShot(colorGot);

			if (color == this.color) {
				colorAmount += amount;
			} else {
				this.color = color;
				colorAmount = amount;

				StopAllCoroutines();
				StartCoroutine(SwitchColorRoutine(color));
			}
			Debug.Log("Crystal Received Color: " + colorAmount + " / " + colorRequired);
		}
	}

	private IEnumerator SwitchColorRoutine(GameColor color) {
		Color startColor = renderer.material.color;
		
		for (float timer = 0f; timer < 2f; timer += Time.deltaTime) {
			SetRendererColors(Color.Lerp(startColor, color.DullColor(), timer / 1f));
			yield return null;
		}
		
		SetRendererColors(color.DullColor());
	}

	private IEnumerator ActivateRoutine(GameColor color) {
		active = true;
		ColorBlobBehaviour blob = (ColorBlobBehaviour) Instantiate(blobPrefab, transform.position + new Vector3(0f, spawnHeight, 0f), Quaternion.identity);
		blob.SetColor(color);
		blob.transform.parent = transform;
		
		Color startColor = renderer.material.color;

		audio.PlayOneShot(colorFilled);
		
		for (float timer = 0f; timer < 2f; timer += Time.deltaTime) {
			SetRendererColors(Color.Lerp(startColor, color.GemColor(), timer / 2f));
			yield return null;
		}
		
		SetRendererColors(color.GemColor());
	}

	private void SetRendererColors(Color color) {
		renderer.material.color = color;
		foreach (Renderer r in otherGems) {
			r.material.color = color;
		}
	}
}
