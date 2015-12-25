using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class ResourceCountPanel : MonoBehaviour {

	public Image image;
	public Text text;

	public void SetContents(ResourceType type, int count)
	{
		// Fill in the contents
		image.color = GetColor(type);
		text.text = count.ToString();
	}

	private Color GetColor(ResourceType resource)
	{
		switch(resource)
		{
		case ResourceType.Energy: return Color.white;
		case ResourceType.Red: return Color.red;
		case ResourceType.Yellow: return Color.yellow;
		case ResourceType.Green: return Color.green;
		case ResourceType.Blue: return Color.blue;
		default: throw new ArgumentException("Illegal resource: " + resource, "resource");
		}
	}
}
