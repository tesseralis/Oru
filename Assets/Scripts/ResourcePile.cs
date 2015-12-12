using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Util;

public class ResourcePile : MonoBehaviour
{
	public ResourceCount[] resources;

	public IDictionary<ResourceType, int> Resources
	{
		get
		{
			return resources.Aggregate(Multiset.Empty<ResourceType>(),
				(ms, resource) => ms.MultisetAdd(resource.type, resource.count));
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
