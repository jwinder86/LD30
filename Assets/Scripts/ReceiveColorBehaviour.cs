using UnityEngine;
using System.Collections;

public class ReceiveColorBehaviour : MonoBehaviour {

	public HandleColorHitBehaviour handler;

	public void ReceiveColor(GameColor color, float amount) {
		handler.HandleColor(color, amount);
	}
}
