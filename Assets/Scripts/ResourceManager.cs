using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Util;

public class ResourceManager : MonoBehaviour
{
	// Get the pile of resources at the given coordinate
	public IDictionary<ResourceType, int> this[Coordinate coordinate]
	{
		get { return locations[coordinate].Resources; }
	}

	public bool Contains(Coordinate coordinate)
	{
		return locations.ContainsKey(coordinate);
	}

	private IDictionary<Coordinate, ResourcePile> locations;

	// Use this for initialization
	void Start () {
		locations = new Dictionary<Coordinate, ResourcePile>();
		// Assume this grid's children are all terrain blocks
		foreach (Transform child in gameObject.transform)
		{
			ResourcePile block = child.gameObject.GetComponent<ResourcePile>();
			locations[block.Coordinate()] = block;
		}
	}

}
