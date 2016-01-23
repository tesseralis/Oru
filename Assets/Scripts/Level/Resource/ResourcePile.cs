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

	// Height between separate resource types
	public float heightGap = 0.1f;

	public ResourceCollection resources;

	public ResourceCollection ResourceCollection
	{
		get { return resources; }
		set
		{
			resources = value;
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
		int numResourceTypes = 0;
		foreach (var resource in ResourceCollection.Paper.Keys)
		{
			var size = 1;
			while (ResourceCollection.Paper[resource] >= size * size)
			{
				numResourceTypes++;
				size ++;
				var prefab = ResourcesPathfinder.PaperResourcePrefab(resource);
				var obj = gameObject.AddChild(prefab, gameObject.Coordinate());

				// Set the height correctly
				obj.transform.Translate(Vector3.up * numResourceTypes * heightGap);

			}
		}

		// Put the energy resource on top of everything else
		if (ResourceCollection.EnergyBlocks.Count > 0)
		{
			numResourceTypes++;
			var maxEnergy = ResourceCollection.EnergyBlocks.Max();
			var prefab = ResourcesPathfinder.EnergyResourcePrefab(maxEnergy);
			var obj = gameObject.AddChild(prefab, gameObject.Coordinate());
			obj.transform.Translate(Vector3.up * numResourceTypes * heightGap);
		}
	}

}
