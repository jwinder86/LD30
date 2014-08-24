using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]
public class ColorBlobBehaviour : MonoBehaviour {

	public GameColor.ColorName colorName;
	public float amount;

	private GameColor color;

	// Use this for initialization
	void Start () {
		color = GameColor.FromName(colorName);
		renderer.material.color = color.GemColor();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider collider) {
		ReceiveColorBehaviour other = collider.GetComponent<ReceiveColorBehaviour>();

		if (other != null) {
			other.ReceiveColor(color, amount);
			Destroy(gameObject);
		}
	}
}
