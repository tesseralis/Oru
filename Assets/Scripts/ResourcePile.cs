using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ResourcePile : MonoBehaviour
{
	public ResourceCount[] resources;

	void OnMouseOver()
	{
		Debug.Log("Going over resources.");
	}
}

[Serializable]
public class ResourceCount
{
	public ResourceType type;
	public int count;
}

public enum ResourceType { Red, Yellow, Green, Blue }
