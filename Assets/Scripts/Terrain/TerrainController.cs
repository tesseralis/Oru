using UnityEngine;
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
	public AudioClip clickAudio;
	public GameObject clickParticlePrefab;
	public TerrainPrefabOptions terrainPrefabs;

	public Action<Coordinate> MouseEnterBlock;
	public Action<Coordinate> MouseExitBlock;
	public Action<Coordinate> ClickBlock;

	private IDictionary<Coordinate, TerrainBlock> grid;

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

			// Create the new item
			var prefab = terrainPrefabs.PrefabFor (value);
			var block = this.AddChildWithComponent<TerrainBlock>(prefab, coordinate);
			block.type = value;
			grid[coordinate] = block;

		}
	}

	public bool Contains(Coordinate coordinate)
	{
		return grid.ContainsKey(coordinate);
	}

	// Initialize the grid
	void Start ()
	{
		grid = new Dictionary<Coordinate, TerrainBlock>();
		foreach (var block in GetComponentsInChildren<TerrainBlock>())
		{
			grid[block.Coordinate()] = block;
		}

		ClickBlock += SpawnParticles;
		ClickBlock += PlaySound;
	}

	void SpawnParticles(Coordinate coordinate)
	{
		// Set the particles to play whenever we click
		if (clickParticlePrefab)
		{
			this.AddChild(clickParticlePrefab, coordinate);
		}
	}

	// Play a sound whenever we click terrain
	void PlaySound(Coordinate coordinate)
	{
		if (clickAudio)
		{
			Camera.main.GetComponent<AudioSource>().PlayOneShot(clickAudio);
		}
	}

}

[Serializable]
public class TerrainPrefabOptions
{
	public GameObject landPrefab;
	public GameObject rockPrefab;
	public GameObject treePrefab;
	public GameObject waterPrefab;


	public GameObject PrefabFor (TerrainType type)
	{
		switch (type) {
		case TerrainType.Land:
			return landPrefab;
		case TerrainType.Rock:
			return rockPrefab;
		case TerrainType.Tree:
			return treePrefab;
		case TerrainType.Water:
			return waterPrefab;
		default:
			throw new ArgumentException("Illegal terrain type: " + type, "type");
		}
	}
}