using UnityEngine;
using System.Collections;

public class HandleColorHitBehaviour : MonoBehaviour {

	public virtual void HandleColor(GameColor color, float amount) {
		Debug.Log("Received color, did nothing: " + color + ", " + amount);
	}
}
