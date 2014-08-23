using UnityEngine;
using System.Collections;

public class TempleBehaviour : MonoBehaviour {

	public Transform gem;
	public MonumentBehaviour[] monuments;
	public GameColor.ColorName colorName;

	private GameColor color;
	private bool charged;

	// Use this for initialization
	void Start () {
		charged = false;
		color = GameColor.FromName(colorName);
	}
	
	// Update is called once per frame
	void Update () {
		if (!charged) {
			bool allCharged = true;
			foreach ( MonumentBehaviour monument in monuments) {
				allCharged = allCharged && monument.IsCharged();
			}

			charged = allCharged;
			if (charged) {
				StartCoroutine(ActivateRoutine());
			}
		}
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
