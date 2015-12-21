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
					var resourcePile = gameObject.AddChildWithComponent<ResourcePile>(new GameObject(), coordinate);
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
			locations[pile.gameObject.Coordinate()] = pile;
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

	public GameObject PrefabFor(ResourceType resource)
	{
		switch(resource)
		{
		case ResourceType.Energy:
			return energyPrefab;
		case ResourceType.Red:
			return redPrefab;
		case ResourceType.Yellow:
			return yellowPrefab;
		case ResourceType.Green:
			return greenPrefab;
		case ResourceType.Blue:
			return bluePrefab;
		default:
			throw new ArgumentException("Resource type not supported: " + resource, "resource");
		}
	}
}