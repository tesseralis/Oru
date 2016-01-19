using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Util;

public class ResourceController : MonoBehaviour
{
	// the prefabs to use to initialize different types of resources
	public ResourcePrefabOptions resourcePrefabs;
	public EnergyPrefabOptions energyPrefabs;

	public GameObject resourcePilePrefab;

	// Get the pile of resources at the given coordinate
	public ResourceCollection this[Coordinate coordinate]
	{
		get
		{
			if (locations.ContainsKey(coordinate))
			{
				return locations[coordinate].Resources;
			}
			else
			{
				return ResourceCollection.Empty();
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
					var resourcePile = gameObject.AddChildWithComponent<ResourcePile>(resourcePilePrefab, coordinate);
					locations[coordinate] = resourcePile;
				}
				locations[coordinate].Resources = value;
			}
		}
	}

	private IDictionary<Coordinate, ResourcePile> locations;

	void Awake ()
	{
		locations = new Dictionary<Coordinate, ResourcePile>();
	}

	// Use this for initialization
	void Start ()
	{
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

[Serializable]
public class EnergyPrefabOptions
{
	public int lowThreshold = 5;
	public GameObject fullEnergyPrefab;
	public GameObject mediumEnergyPrefab;
	public GameObject lowEnergyPrefab;
	public GameObject noEnergyPrefab;

	public GameObject PrefabFor(int energy)
	{
		if (energy <= 0)
		{
			return noEnergyPrefab;
		}
		else if (energy < lowThreshold)
		{
			return lowEnergyPrefab;
		}
		else if (energy < LevelManager.Creatures.maxHealth)
		{
			return mediumEnergyPrefab;
		}
		else
		{
			return fullEnergyPrefab;
		}
	}
}
