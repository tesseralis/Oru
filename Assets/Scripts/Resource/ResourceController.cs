using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Util;

public class ResourceController : MonoBehaviour
{
	// the prefabs to use to initialize different types of resources
	public ResourcePrefabOptions resourcePrefabs;

	// Get the pile of resources at the given coordinate
	public IDictionary<ResourceType, int> this[Coordinate coordinate]
	{
		get
		{
			if (locations.ContainsKey(coordinate))
			{
				return locations[coordinate].Resources;
			}
			else
			{
				return Multiset.Empty<ResourceType>();
			}
		}
		set
		{
			// Destroy the location if it is empty
			if (value.IsEmpty())
			{
				if (locations.ContainsKey(coordinate)) {
					Destroy(locations[coordinate].gameObject);
					locations.Remove(coordinate);
				}
			}
			else
			{
				if (!locations.ContainsKey(coordinate))
				{
					// Instantiate a new marker
					var resourcePile = this.AddChildWithComponent<ResourcePile>(new GameObject(), coordinate);
					locations[coordinate] = resourcePile;
				}
				locations[coordinate].Resources = value;
			}
		}
	}

	private IDictionary<Coordinate, ResourcePile> locations;

	// Use this for initialization
	void Start () {
		locations = new Dictionary<Coordinate, ResourcePile>();
		// Assume this grid's children are all terrain blocks
		foreach (var pile in GetComponentsInChildren<ResourcePile>())
		{
			locations[pile.Coordinate()] = pile;
		}
	}


	// TODO should this be a method of the Options class?
	public GameObject PrefabFor(ResourceType resource)
	{
		switch(resource)
		{
		case ResourceType.Energy:
			return resourcePrefabs.energyPrefab;
		case ResourceType.Red:
			return resourcePrefabs.redPrefab;
		case ResourceType.Yellow:
			return resourcePrefabs.yellowPrefab;
		case ResourceType.Green:
			return resourcePrefabs.greenPrefab;
		case ResourceType.Blue:
			return resourcePrefabs.bluePrefab;
		default:
			throw new ArgumentException("Resource type not supported: " + resource, "resource");
		}
	}

}

[Serializable]
public class ResourcePrefabOptions
{
	public GameObject energyPrefab;
	public GameObject redPrefab;
	public GameObject yellowPrefab;
	public GameObject bluePrefab;
	public GameObject greenPrefab;
}