﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
	// Get the pile of resources at the given coordinate
	public ResourceCount[] this[Coordinate coordinate]
	{
		get { return locations[coordinate].resources; }
	}

	private IDictionary<Coordinate, ResourcePile> locations;

	// Use this for initialization
	void Start () {
		// TODO this might fail if any other "start" function requires the grid
		locations = new Dictionary<Coordinate, ResourcePile>();
		// Assume this grid's children are all terrain blocks
		foreach (Transform child in gameObject.transform)
		{
			ResourcePile block = child.gameObject.GetComponent<ResourcePile>();
			locations[block.Coordinate] = block;
		}
	}

}