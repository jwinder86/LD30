using UnityEngine;
using System.Collections;

public class TempleBehaviour : MonoBehaviour {

	public Transform gem;
	public Transform [] lights;
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

		for (int i = 0; i < lights.Length; i++) {
			if (i >= monuments.Length) {
				lights[i].renderer.enabled = false;
			} else {
				lights[i].renderer.material.color = monuments[i].GetCurrentColor();
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
