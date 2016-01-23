using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Util;

public class ResourceController : MonoBehaviour
{
	// Get the pile of resources at the given coordinate
	public ResourceCollection this[Coordinate coordinate]
	{
		get
		{
			if (locations.ContainsKey(coordinate))
			{
				return locations[coordinate].ResourceCollection;
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
					locations[coordinate] = AddResourcePile(coordinate, value);
				}
				else
				{
					locations[coordinate].ResourceCollection = value;
				}
			}
		}
	}

	// Add a resource pile object, but do not change the game state
	public ResourcePile AddResourcePile(Coordinate coordinate, ResourceCollection resources)
	{
		var prefab = ResourcesPathfinder.ResourcePilePrefab();
		var resourcePile = gameObject.AddChildWithComponent<ResourcePile>(prefab, coordinate);
		resourcePile.resources = resources;
		return resourcePile;
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
