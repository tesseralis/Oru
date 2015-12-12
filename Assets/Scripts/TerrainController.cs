﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Util;

/// <summary>
/// Manages the overall state of the terrain in the game,
/// and the interaction between the terrain blocks and other
/// game elements.
/// </summary>
public class TerrainController : MonoBehaviour
{
	// TODO make this a dropdown class
	public GameObject grassPrefab;
	public GameObject rockPrefab;
	public GameObject treePrefab;
	public GameObject waterPrefab;

	public Action<Coordinate> OnHover;
	public Action<Coordinate> OnClick;

	public TerrainType this[Coordinate coordinate]
	{
		get
		{
			return grid[coordinate].type;
		}
		set
		{
			// Delete the old value
			Destroy(grid[coordinate].gameObject);

			// TODO factor this out into a method
			// Set the new value
			GameObject prefab;
			switch(value)
			{
			case TerrainType.Grass:
			default:
				prefab = grassPrefab;
				break;
			case TerrainType.Rock:
				prefab = rockPrefab;
				break;
			}

			GameObject terrainObject = (GameObject)Instantiate(prefab);
			GameManager.gm.SetPosition(terrainObject, coordinate);
			terrainObject.transform.parent = gameObject.transform;

			if (!terrainObject.GetComponent<TerrainBlock>())
			{
				var block = terrainObject.AddComponent<TerrainBlock>();
				block.type = value;
			}

			grid[coordinate] = terrainObject.GetComponent<TerrainBlock>();

		}
	}

	public bool Contains(Coordinate coordinate)
	{
		return grid.ContainsKey(coordinate);
	}

	private IDictionary<Coordinate, TerrainBlock> grid;

	// Initialize the grid
	void Start ()
	{
		grid = new Dictionary<Coordinate, TerrainBlock>();
		foreach (var block in GetComponentsInChildren<TerrainBlock>())
		{
			grid[block.Coordinate()] = block;
		}
	}

}