using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainTowerBehaviour : MonoBehaviour {

	public LineBehaviour faintLinePrefab;
	public LineBehaviour boldLinePrefab;

	public GameColor.ColorName[] colorNames;
	public GemBehaviour[] gems;
	public TempleBehaviour[] temples;

	public Transform endPosition;
	public GameObject endPrefab;
	private bool endCreated;

	private Dictionary<GameColor, GemBehaviour> gemMap;
	private Dictionary<GameColor, bool> gemActive;
	private Dictionary<GameColor, TempleBehaviour> templeMap;
	private Dictionary<GameColor, LineBehaviour> faintLines;
	private Dictionary<GameColor, LineBehaviour> boldLines;

	// Use this for initialization
	void Start () {
		gemMap = new Dictionary<GameColor, GemBehaviour>();
		gemActive = new Dictionary<GameColor, bool>();
		templeMap = new Dictionary<GameColor, TempleBehaviour>();
		faintLines = new Dictionary<GameColor, LineBehaviour>();
		boldLines = new Dictionary<GameColor, LineBehaviour>();

		for (int i = 0; i < colorNames.Length; i++) {
			GameColor color = GameColor.FromName(colorNames[i]);
			gemMap[color] = gems[i];
			gemActive[color] = false;
			gems[i].SetColor(color.DullColor());

			if (temples[i]) {
				templeMap[color] = temples[i];
			}
		}

		endCreated = false;
	}
	
	// Update is called once per frame
	void Update () {
		// Green Logic
		if (templeMap[GameColor.Green].IsCharged()) {
			EnsureLine(GameColor.Green, true);
			EnsureGemActive(GameColor.Green);
		} else {
			EnsureLine(GameColor.Green, false);
		}

		// Cyan Logic
		if (templeMap[GameColor.Cyan].IsCharged()) {
			EnsureLine(GameColor.Cyan, true);
			EnsureGemActive(GameColor.Cyan);
		} else if (templeMap[GameColor.Green].IsCharged()){
			EnsureLine(GameColor.Cyan, false);
		}

		// Blue Logic
		if (templeMap[GameColor.Blue].IsCharged()) {
			EnsureLine(GameColor.Blue, true);
			EnsureGemActive(GameColor.Blue);
		} else if (templeMap[GameColor.Green].IsCharged() && templeMap[GameColor.Cyan].IsCharged()){
			EnsureLine(GameColor.Blue, false);
		}

		// Red Logic
		if (templeMap[GameColor.Red].IsCharged()) {
			EnsureLine(GameColor.Red, true);
			EnsureGemActive(GameColor.Red);
		} else if (templeMap[GameColor.Green].IsCharged() && templeMap[GameColor.Cyan].IsCharged()){
			EnsureLine(GameColor.Red, false);
		}

		// Yellow Logic
		if (templeMap[GameColor.Yellow].IsCharged()) {
			EnsureLine(GameColor.Yellow, true);
			EnsureGemActive(GameColor.Yellow);
		} else if (templeMap[GameColor.Red].IsCharged() && templeMap[GameColor.Blue].IsCharged() && templeMap[GameColor.Cyan].IsCharged()){
			EnsureLine(GameColor.Yellow, false);
		}

		// Purple Logic
		if (templeMap[GameColor.Purple].IsCharged()) {
			EnsureLine(GameColor.Purple, true);
			EnsureGemActive(GameColor.Purple);
		} else if (templeMap[GameColor.Red].IsCharged() && templeMap[GameColor.Blue].IsCharged() && templeMap[GameColor.Yellow].IsCharged()){
			EnsureLine(GameColor.Purple, false);
		}

		// End Logic
		if (templeMap[GameColor.Purple].IsCharged() && !endCreated) {
			Instantiate(endPrefab, endPosition.position, Quaternion.identity);
			endCreated = true;
		}
	}

	private void EnsureGemActive(GameColor color) {
		if (!gemActive[color]) {
			gemActive[color] = true;
			gemMap[color].StartAnim();
			gemMap[color].SetColor(color.GemColor());
		}
	}

	private void EnsureLine(GameColor color, bool bold) {
		LineBehaviour faintLine = faintLines.ContainsKey(color) ? faintLines[color] : null;
		LineBehaviour boldLine = boldLines.ContainsKey(color) ? boldLines[color] : null;

		if (bold) {
			if (faintLine != null) {
				faintLine.Stop();
				faintLines.Remove(color);
			}

			if (boldLine == null) {
				boldLine = CreateBoldLine(color);
				boldLines[color] = boldLine;
			}
		} else {
			if (faintLine == null) {
				faintLine = CreateFaintLine(color);
				faintLines[color] = faintLine;
			}
		}
	}

	private LineBehaviour CreateFaintLine(GameColor color) {
		Vector3 start = gemMap[color].transform.position;
		Vector3 end = templeMap[color].GetGemPosition();
		LineBehaviour line = (LineBehaviour) Instantiate(faintLinePrefab, start, Quaternion.identity);
		line.Configure(start, end, new Color(1f, 1f, 1f, 0.3f), 20f, 2f, 2f);
		return line;
	}

	private LineBehaviour CreateBoldLine(GameColor color) {
		Vector3 end = gemMap[color].transform.position;
		Vector3 start = templeMap[color].GetGemPosition();
		LineBehaviour line = (LineBehaviour) Instantiate(boldLinePrefab, start, Quaternion.identity);
		Color lineColor = color.GemColor();
		lineColor.a = 0.25f;
		line.Configure(start, end, lineColor, 30f, 1f, 0.5f);
		return line;
	}
}
