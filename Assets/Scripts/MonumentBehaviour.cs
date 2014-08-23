using UnityEngine;
using System.Collections;

public class MonumentBehaviour : HandleColorHitBehaviour {

	public float amountRequired;
	public GameColor.ColorName colorName;
	public Transform bar;
	public Transform gem;

	private GameColor color;
	private float filled;

	// Use this for initialization
	void Start () {
		color = GameColor.FromName(colorName);
		filled = 0f;

		gem.renderer.material.color = color.DullColor();
		bar.renderer.material.color = color.GemColor();
	}
	
	// Update is called once per frame
	void Update () {
		float newSize = Mathf.MoveTowards(bar.localScale.x, filled / amountRequired, 0.1f);
		bar.localScale = new Vector3(newSize, 1f, 1f);
	}

	public override void HandleColor(GameColor color, float amount) {
		if (this.color == color && filled < amountRequired) {
			filled += amount;

			if (filled >= amountRequired) {
				filled = amountRequired;
				StartCoroutine(ActivateRoutine());
			}

			Debug.Log("Filled with " + color + ", " + amount + "; " + filled + " / " + amountRequired);
		} else {
			Debug.Log("Wrong color: " + color + ", needed: " + this.color);
		}
	}

	public bool IsCharged() {
		return filled >= amountRequired;
	}

	private IEnumerator ActivateRoutine() {
		animation.Play();

		Color startColor = gem.renderer.material.color;

		for (float timer = 0f; timer < 2f; timer += Time.deltaTime) {
			gem.renderer.material.color = Color.Lerp(startColor, color.GemColor(), timer / 2f);
			yield return null;
		}

		gem.renderer.material.color = color.GemColor();
	}
}
