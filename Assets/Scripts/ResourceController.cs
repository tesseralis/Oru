using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Util;

public class ResourceController : MonoBehaviour
{
	// The prefab to use to initialize a new marker.
	public GameObject resourcePilePrefab;

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
					// TODO factor out all this component creation logic in terrain, resource, and creature
					var resourcePileObject = Instantiate(resourcePilePrefab);
					GameManager.gm.SetPosition(resourcePileObject, coordinate);
					var resourcePile = resourcePileObject.GetComponent<ResourcePile>();
					if (resourcePile == null)
					{
						resourcePile = resourcePileObject.AddComponent<ResourcePile>();
					}
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

}
