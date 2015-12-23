using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class ResourceCountPanel : MonoBehaviour {

	public void SetContents(ResourceType type, int count)
	{
		Debug.Log(type + " " + count);
		// Fill in the contents
		// TODO don't rely on the panel having this shape
		GetComponentInChildren<Image>().color = GetColor(type);
		GetComponentInChildren<Text>().text = count.ToString();
	}

	private Color GetColor(ResourceType resource)
	{
		switch(resource)
		{
		case ResourceType.Energy: return Color.white;
		case ResourceType.Red: return Color.blue;
		case ResourceType.Yellow: return Color.yellow;
		case ResourceType.Green: return Color.green;
		case ResourceType.Blue: return Color.blue;
		default: throw new ArgumentException("Illegal resource: " + resource, "resource");
		}
	}
}
