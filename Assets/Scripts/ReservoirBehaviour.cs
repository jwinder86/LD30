using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReservoirBehaviour : MonoBehaviour {
	
	public float tankSize = 10f;

	public Texture crosshairTex;
	public float crosshairHeightRatio;

	private Dictionary<GameColor, float> tanks;
	private GameColor currentColor;
	private GameColor lastColor;

	// Use this for initialization
	void Start () {
		tanks = new Dictionary<GameColor, float>();

		Screen.showCursor = false;

		currentColor = GameColor.Green;
		tanks[currentColor] = tankSize;
		tanks[GameColor.Red] = tankSize;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Mouse ScrollWheel") > 0) {
			SwitchColor(false);
		} else if (Input.GetAxis("Mouse ScrollWheel") < 0) {
			SwitchColor(true);
		}
	}

	private void SwitchColor(bool inverse) {
		Debug.Log("Switching colors " + (inverse ? "" : "inverted"));
		if (tanks.Keys.Count == 0) {
			currentColor = null;
			lastColor = null;
		} else {
			currentColor = currentColor == null ? lastColor : currentColor;
			currentColor = currentColor == null ? GameColor.Red : currentColor;
			currentColor = currentColor.NextColor(inverse);

			while (!tanks.ContainsKey(currentColor)) {
				currentColor = currentColor.NextColor(inverse);
			}
		}
	}

	public float GetColorFromTank(float desiredAmount) {
		float output;

		if (currentColor == null) {
			return 0f;
		} else if (tanks[currentColor] > desiredAmount) {
			output = desiredAmount;
			tanks[currentColor] -= desiredAmount;
		} else {
			output = tanks[currentColor];
			tanks.Remove(currentColor);
			lastColor = currentColor;
			currentColor = null;
		}

		return output;
	}

	public GameColor GetCurrentColor() {
		if (currentColor != null) {
			return currentColor;
		} else {
			return lastColor;
		}
	}

	void OnGUI() {
		float crosshairRadius = crosshairHeightRatio * Screen.height / 2f;
		Vector3 pos = Input.mousePosition;
		
		// draw background
		GUI.color = new Color(1f, 1f, 1f, 0.1f);
		GUI.DrawTexture(new Rect(pos.x - crosshairRadius, Screen.height - pos.y - crosshairRadius, crosshairRadius * 2f, crosshairRadius * 2f), crosshairTex, ScaleMode.StretchToFill, true);

		// draw color amount
		if (currentColor != null) {
			float colorRadius = Mathf.Sqrt(tanks[currentColor] / tankSize) * crosshairRadius;
			GUI.color = currentColor.GuiColor();
			GUI.DrawTexture(new Rect(pos.x - colorRadius, Screen.height - pos.y - colorRadius, colorRadius * 2f, colorRadius * 2f), crosshairTex, ScaleMode.StretchToFill, true);
		}
	}
}
