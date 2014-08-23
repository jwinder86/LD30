﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameColor {
	public enum ColorName { Red, Green, Blue, Yellow, Orange, Purple };

	public static GameColor Red = new GameColor(ColorName.Red);
	public static GameColor Green = new GameColor(ColorName.Green);
	public static GameColor Blue = new GameColor(ColorName.Blue);
	public static GameColor Yellow = new GameColor(ColorName.Yellow);
	public static GameColor Orange = new GameColor(ColorName.Orange);
	public static GameColor Purple = new GameColor(ColorName.Purple);

	private static Dictionary<ColorName, GameColor> colorByName = new Dictionary<ColorName, GameColor>() {
		{ColorName.Red, Red},
		{ColorName.Green, Green},
		{ColorName.Blue, Blue},
		{ColorName.Yellow, Yellow},
		{ColorName.Orange, Orange},
		{ColorName.Purple, Purple}
	};

	private static Dictionary<ColorName, Color> guiColors = new Dictionary<ColorName, Color>() {
		{ColorName.Green, new Color(0.1f, 0.9f, 0.1f, 0.5f)},
		{ColorName.Red, new Color(0.9f, 0.1f, 0.1f, 0.5f)}
	};

	private static Dictionary<ColorName, Color> worldColors = new Dictionary<ColorName, Color>() {
		{ColorName.Green, new Color(0.1f, 0.9f, 0.1f, 1f)},
		{ColorName.Red, new Color(0.9f, 0.1f, 0.1f, 1f)}
	};

	private static Dictionary<ColorName, Color> dullColors = new Dictionary<ColorName, Color>() {
		{ColorName.Green, new Color(0.3f, 0.4f, 0.3f, 1f)},
		{ColorName.Red, new Color(0.4f, 0.3f, 0.3f, 1f)}
	};

	private static Dictionary<ColorName, Color> gemColors = new Dictionary<ColorName, Color>() {
		{ColorName.Green, new Color(0.4f, 1f, 0.4f, 1f)},
		{ColorName.Red, new Color(1f, 0.4f, 0.4f, 1f)}
	};

	private static List<GameColor> colorOrder = new List<GameColor>(new GameColor[] {
		GameColor.Red,
		GameColor.Orange,
		GameColor.Yellow,
		GameColor.Green,
		GameColor.Blue,
		GameColor.Purple
	});

	private ColorName color;

	private GameColor(ColorName color) {
		this.color = color;
	}

	public static GameColor FromName(ColorName name) {
		return colorByName[name];
	}

	public Color GuiColor() {
		return guiColors[color];
	}

	public Color Color() {
		return worldColors[color];
	}

	public Color DullColor() {
		return dullColors[color];
	}

	public Color GemColor() {
		return gemColors[color];
	}

	public GameColor NextColor(bool inverse) {
		for (int i = 0; i < colorOrder.Count; i++) {
			if (colorOrder[i].color == this.color) {
				if (inverse) {
					Debug.Log("Returning inverse");
					return colorOrder[(i + 1) % colorOrder.Count];
				} else {
					Debug.Log("Returning next");
					return colorOrder[(i + colorOrder.Count - 1) % colorOrder.Count];
				}
			}
		}

		Debug.Log("Couldn't find next color");
		return null;
	}

	public override string ToString() {
		return color.ToString();
	}
}