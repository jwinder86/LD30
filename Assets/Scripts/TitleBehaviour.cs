using UnityEngine;
using System.Collections;

public class TitleBehaviour : MonoBehaviour {

	public float fadeTime = 2f;
	public bool fadeInAwake;
	public Texture2D blank;

	private bool starting;
	private float fadeTimer;
	private bool fadeIn;

	// Use this for initialization
	void Start () {
		starting = false;

		if (fadeInAwake) {
			fadeIn = true;
			fadeTimer = fadeTime;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!starting && 
		    (Input.GetButtonDown("Fire1") || 
		    Input.GetButtonDown("Fire2"))) {
			StartCoroutine(StartRoutine());
		}

		fadeTimer -= Time.deltaTime;
	}

	void OnGUI() {
		// set the color of the GUI
		Color guiColor = Color.black;
		
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

	private IEnumerator StartRoutine() {
		fadeIn = false;
		fadeTimer = fadeTime;

		yield return new WaitForSeconds(fadeTime);

		Application.LoadLevel("TestScene");
	}
}
