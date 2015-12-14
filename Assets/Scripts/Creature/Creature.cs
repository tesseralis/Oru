﻿using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A creature represents a single movable unit in the game, whether friendly or not.
/// </summary>
public class Creature : MonoBehaviour
{
	// The type of creature this is.
	public CreatureType creatureType;

	public Coordinate? Goal { get; set; }

	public Coordinate Position
	{
		get { return position; }
	}

	public Action<Coordinate?> OnSetGoal;

	private CreatureController manager;

	private Coordinate position;

	void Start()
	{
		// Store our initial position
		position = GameManager.gm.ToGridCoordinate(gameObject.transform.position);
	}

	void OnMouseDown()
	{
		GetComponentInParent<CreatureController>().OnSelect(this);
	}

	public void Step()
	{
		if (Goal != null && (!position.Equals(Goal)))
		{
			var next = NextCoordinate ();

			// Make our creature face the right direction
			if (next != position)
			{
				var direction = next - position;
				transform.Rotate(new Vector3(0, AngleFor(direction)) - transform.rotation.eulerAngles);
			}

			// Update our creature's position
			position = next;


			// Update the position visually
			GameManager.gm.SetPosition(gameObject, next);
		}
	}

	private float AngleFor(Coordinate direction)
	{
		if (direction == Coordinate.up) { return -90; }
		if (direction == Coordinate.right) { return 0; }
		if (direction == Coordinate.down) { return 90; }
		if (direction == Coordinate.left) { return 180; }
		throw new ArgumentException("Given coordinate" + direction + " is not a direction", "direction");
	}

	// do a BFS and figure out the right path
	private Coordinate NextCoordinate()
	{
		// TODO do A* search
		// If we can't get to the destination, just search for our own location
		var goal = Goal ?? Position;
		var grid = GameManager.Terrain;
		var allowedTerrain = Creatures.ForType(creatureType).AllowedTerrain;

		var distance = new Dictionary<Coordinate, int>();
		var parents = new Dictionary<Coordinate, Coordinate> ();
		var queue = new Queue<Coordinate> ();
		distance [position] = 0;
		queue.Enqueue (position);
		
		while (queue.Count > 0)
		{
			var current = queue.Dequeue();
			var neighbors = Coordinate.cardinals.Select(x => x + current).ToList();
			foreach (Coordinate neighbor in neighbors)
			{
				if (grid.Contains(neighbor) && allowedTerrain.Contains(grid[neighbor]) && !distance.ContainsKey(neighbor))
				{
					distance[neighbor] = distance[current] + 1;
					parents[neighbor] = current;
					queue.Enqueue(neighbor);
				}
			}
			// If we've already found our goal, we do not need to keep searching.
			if (neighbors.Contains(goal))
			{
				break;
			}
			
		}

		var next = goal;
		if (!parents.ContainsKey(next))
		{
			Debug.Log("Can't reach the intended destination.");
			return position;
		}
		while (parents[next] != (position))
		{
			next = parents[next];
		}
		return next;
	}

}
