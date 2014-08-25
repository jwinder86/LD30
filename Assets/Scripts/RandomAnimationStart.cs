using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Animation))]
public class RandomAnimationStart : MonoBehaviour {

	public float maxDelay = 4f;

	// Use this for initialization
	void Start () {
		StartCoroutine(WaitRoutine(Random.Range(0f, maxDelay)));
	}
	
	private IEnumerator WaitRoutine(float delay) {
		yield return new WaitForSeconds(delay);
		animation.Play();
	}
}
