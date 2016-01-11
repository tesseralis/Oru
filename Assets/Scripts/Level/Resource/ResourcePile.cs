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

	public ResourceCollection Resources
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
		var controller = GetComponentInParent<ResourceController>();
		int numResourceTypes = 0;
		foreach (var resource in Resources.Paper.Keys)
		{
			numResourceTypes++;
			var obj = gameObject.AddChild(controller.resourcePrefabs.PrefabFor(resource), gameObject.Coordinate());

			// Set the height correctly
			obj.transform.Translate(Vector3.up * numResourceTypes * heightGap);
		}

		// Put the energy resource on top of everything else
		// TODO show different things based on the health of the energy block
		if (Resources.EnergyBlocks.Count > 0)
		{
			numResourceTypes++;
			var maxEnergy = Resources.EnergyBlocks.Max();
			var obj = gameObject.AddChild(controller.energyPrefabs.PrefabFor(maxEnergy), gameObject.Coordinate());
			obj.transform.Translate(Vector3.up * numResourceTypes * heightGap);
		}
	}
}
