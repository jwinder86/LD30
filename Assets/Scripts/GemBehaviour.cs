using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Animation))]
public class GemBehaviour : MonoBehaviour {

	public Renderer renderer;

	public Color GetColor() {
		return renderer.material.color;
	}

	public void SetColor(Color color) {
		renderer.material.color = color;
	}

	public void StartAnim() {
		animation.Play();
	}

	public void StopAnim() {
		animation.Stop();
	}
}
