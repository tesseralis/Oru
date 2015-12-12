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
		set
		{
			resources = value.Select(resource => new ResourceCount(resource.Key, resource.Value)).ToArray();
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
	public ResourceCount(ResourceType _type, int _count)
	{
		type = _type;
		count = _count;
	}

	public ResourceType type;
	public int count;
}
