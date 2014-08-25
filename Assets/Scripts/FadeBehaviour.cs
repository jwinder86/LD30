﻿using UnityEngine;
using System.Collections;

public class FadeBehaviour : MonoBehaviour {

	public Texture2D blank;
	public float fadeTime;
	public bool fadeOnAwake;
	
	private Color fadeColor = Color.black;
	private float fadeTimer;
	private bool fadeIn;
	
	// Use this for initialization
	void Start () {
		if (fadeOnAwake) {
			fadeTimer = fadeTime;
			fadeIn = true;
		} else {
			fadeTimer = 0f;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (fadeTimer > 0f) {
			fadeTimer -= Time.deltaTime;
			Debug.Log("Running Fade: " + fadeTimer );
		}
	}
	
	void OnGUI() {
		// set the color of the GUI
		Color guiColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b);
		
		// interpolate the alpha of the GUI from 1(fully visible)
		// to 0(invisible) over time
		if (fadeIn) {
			guiColor.a = Mathf.Lerp(0f, 1f, (fadeTimer / fadeTime));
		} else {
			guiColor.a = Mathf.Lerp(1f, 0f, (fadeTimer / fadeTime));
		}
		
		GUI.color = guiColor;
		
		// draw the texture to fill the screen
		GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), blank);
	}
	
	public void FadeIn() {
		fadeTimer = fadeTime;
		fadeIn = true;
	}
	
	public void FadeOut() {
		Debug.Log("Fade out");
		fadeTimer = fadeTime;
		fadeIn = false;
	}
}
