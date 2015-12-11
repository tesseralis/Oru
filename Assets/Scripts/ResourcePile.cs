using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ResourcePile : MonoBehaviour
{
	public ResourceCount[] resources;

	public IDictionary<ResourceType, int> Resources
	{
		get
		{
			var result = new Dictionary<ResourceType, int>();
			foreach(var resource in resources)
			{
				if (result.ContainsKey(resource.type))
				{
					result[resource.type] += resource.count;
				}
				else
				{
					result[resource.type] = resource.count;
				}
			}
			return result;
		}
	}

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
