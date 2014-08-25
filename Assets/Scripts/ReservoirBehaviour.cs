using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(AudioSource))]
public class ReservoirBehaviour : HandleColorHitBehaviour {
	
	public float tankSize = 10f;

	public Texture circleTex;
	public Texture crosshairTex;
	public float crosshairHeightRatio;
	public float iconHeightRatio;

	public bool giveAll = false;

	public AudioClip gotColor;

	private Dictionary<GameColor, float> tanks;
	private GameColor currentColor;
	private GameColor lastColor;

	private ShipBehaviour ship;

	// Use this for initialization
	void Start () {
		ship = GetComponent<ShipBehaviour>();

		tanks = new Dictionary<GameColor, float>();

		Screen.showCursor = false;

		if (giveAll) {
			currentColor = GameColor.Green;
			tanks[GameColor.Red] = tankSize;
			tanks[GameColor.Green] = tankSize;
			tanks[GameColor.Blue] = tankSize;
			tanks[GameColor.Yellow] = tankSize;
			tanks[GameColor.Purple] = tankSize;
			tanks[GameColor.Cyan] = tankSize;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!ship.IsStunned()) {
			if (Input.GetAxis("Mouse ScrollWheel") > 0) {
				SwitchColor(true);
			} else if (Input.GetAxis("Mouse ScrollWheel") < 0) {
				SwitchColor(false);
			}
		}
	}

	public override void HandleColor(GameColor color, float amount) {
		audio.PlayOneShot(gotColor);

		if (!tanks.ContainsKey(color)) {
			tanks[color] = Mathf.Min(amount, tankSize);
		} else {
			tanks[color] = Mathf.Min(tanks[color] + amount, tankSize);
		}

		if (currentColor == null) {
			currentColor = color;
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
		float iconRadius = iconHeightRatio * Screen.height / 2f;
		Vector3 pos = Input.mousePosition;
		
		// draw background
		GUI.color = new Color(1f, 1f, 1f, 0.1f);
		GUI.DrawTexture(new Rect(pos.x - crosshairRadius, Screen.height - pos.y - crosshairRadius, crosshairRadius * 2f, crosshairRadius * 2f), circleTex, ScaleMode.StretchToFill, true);

		// draw color amount
		if (currentColor != null) {
			float colorRadius = (tanks[currentColor] / tankSize) * crosshairRadius;
			GUI.color = currentColor.GuiColor();
			GUI.DrawTexture(new Rect(pos.x - colorRadius, Screen.height - pos.y - colorRadius, colorRadius * 2f, colorRadius * 2f), circleTex, ScaleMode.StretchToFill, true);
		}

		// draw crosshair
		GUI.color = Color.white;
		GUI.DrawTexture(new Rect(pos.x - crosshairRadius, Screen.height - pos.y - crosshairRadius, crosshairRadius * 2f, crosshairRadius * 2f), crosshairTex, ScaleMode.StretchToFill, true);

		// draw other colors
		GameColor nextColor = GameColor.Purple;
		for (int i = 0; i < 6; i++) {
			nextColor = nextColor.NextColor(false);
			if (tanks.ContainsKey(nextColor)) {
				float angle = ((float) i / 6f) * Mathf.PI + Mathf.PI / 12f + Mathf.PI;
				Vector3 iconPos = pos + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * (crosshairRadius + iconRadius);

				float colorRadius = (tanks[nextColor] / tankSize) * iconRadius;
				GUI.color = nextColor.GuiColor();
				GUI.DrawTexture(new Rect(iconPos.x - colorRadius, Screen.height - iconPos.y - colorRadius, colorRadius * 2f, colorRadius * 2f), circleTex, ScaleMode.ScaleToFit, true);
			}
		}
	}
}
