using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class TempleBehaviour : MonoBehaviour {

	public LineBehaviour linePrefab;

	public Transform gem;
	public Transform[] lights;
	public Transform[] positions;
	public Transform gemPosition;
	public ColorBlobBehaviour colorBlobPrefab;
	public MonumentBehaviour[] monuments;
	public GameColor.ColorName colorName;

	public AudioClip powerup;

	private GameColor color;
	private bool charged;
	private LineBehaviour[] lines;

	// Use this for initialization
	void Start () {
		charged = false;
		color = GameColor.FromName(colorName);
		gem.renderer.material.color = color.DullColor();

		lines = new LineBehaviour[monuments.Length];
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

				if (monuments[i].IsCharged() && lines[i] == null) {
					LineBehaviour line = (LineBehaviour) Instantiate(linePrefab);
					Color lineColor = monuments[i].GetCurrentColor();
					lineColor.a = 0.25f;
					line.Configure(monuments[i].GetLinePosition(), positions[i].position, lineColor, 10f, 2f, 0.5f);
					lines[i] = line;
				}
			}
		}
	}

	public bool IsCharged() {
		return charged;
	}

	public Vector3 GetGemPosition() {
		return gemPosition.position;
	}

	private IEnumerator ActivateRoutine() {
		ColorBlobBehaviour blob = (ColorBlobBehaviour) Instantiate(colorBlobPrefab, gemPosition.position + new Vector3(0f, 10f, 0f), Quaternion.identity);
		blob.SetColor(color);

		animation.Play();

		audio.PlayOneShot(powerup);
		
		Color startColor = gem.renderer.material.color;
		
		for (float timer = 0f; timer < 2f; timer += Time.deltaTime) {
			gem.renderer.material.color = Color.Lerp(startColor, color.GemColor(), timer / 2f);
			yield return null;
		}
		
		gem.renderer.material.color = color.GemColor();
	}
}
