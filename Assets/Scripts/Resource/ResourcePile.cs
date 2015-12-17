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

	// TODO make this publicly alterable
	// Find the right height for things
	private float heightGap = 0.1f;

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
			if (resource == ResourceType.Energy)
			{
				// Handle the energy differently: put it on top of the rest
				hasEnergy = true;
				continue;
			}
			numResourceTypes++;
			// TODO need to refactor this to not require a component to be attached
			var obj = this.AddChildWithComponent<MonoBehaviour>(controller.PrefabFor(resource), this.Coordinate());

			// Set the height correctly
			obj.transform.Translate(Vector3.up * numResourceTypes * heightGap);
//			var position = obj.gameObject.transform.position;
//			obj.gameObject.transform.position.Set(position.x, numResourceTypes * heightGap, position.z);
//			obj.gameObject.transform.position.y = numResourceTypes * heightGap;
		}

		if (hasEnergy)
		{
			// Put the energy resource on top of everything else
			numResourceTypes++;
			var obj = this.AddChildWithComponent<MonoBehaviour>(controller.PrefabFor(ResourceType.Energy), this.Coordinate());
			obj.transform.Translate(Vector3.up * numResourceTypes * heightGap);
//
//			var position = obj.gameObject.transform.position;
//			obj.gameObject.transform.position.Set(position.x, numResourceTypes * heightGap, position.z);
			//			obj.gameObject.transform.position.y = numResourceTypes * heightGap;
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
