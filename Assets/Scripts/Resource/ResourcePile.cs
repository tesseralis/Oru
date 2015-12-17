using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Util;

/// <summary>
/// Class that keeps track of and visually displays a pile of resources.
/// </summary>
public class ResourcePile : MonoBehaviour
{
	public ResourceCount[] resources;

	// Height between separate resource types
	public float heightGap = 0.1f;

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
			UpdateView();
		}
	}

	void Start()
	{
		UpdateView();
	}

	void DeleteChildren()
	{
		var children = new List<GameObject>();
		foreach (Transform child in transform)
		{
			children.Add(child.gameObject);
		}
		children.ForEach(child => Destroy(child));
	}

	void UpdateView()
	{
		DeleteChildren();
		var controller = GetComponentInParent<ResourceController>();
		int numResourceTypes = 0;
		bool hasEnergy = false;
		foreach (var resource in Resources.Keys)
		{
			// Handle the energy differently: put it on top of the rest
			if (resource == ResourceType.Energy)
			{
				hasEnergy = true;
				continue;
			}
			numResourceTypes++;
			var obj = this.AddChild(controller.PrefabFor(resource), this.Coordinate());

			// Set the height correctly
			obj.transform.Translate(Vector3.up * numResourceTypes * heightGap);
		}

		// Put the energy resource on top of everything else
		if (hasEnergy)
		{
			numResourceTypes++;
			var obj = this.AddChild(controller.PrefabFor(ResourceType.Energy), this.Coordinate());
			obj.transform.Translate(Vector3.up * numResourceTypes * heightGap);
		}
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
