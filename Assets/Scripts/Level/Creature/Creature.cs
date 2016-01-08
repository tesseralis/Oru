using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Util;

/// <summary>
/// A creature represents a single movable unit in the game, whether friendly or not.
/// </summary>
public class Creature : MonoBehaviour
{
	// The type of creature this is.
	public CreatureType creatureType;
	public int health = ResourceCollection.maxHealth;

	public Coordinate Position { get; private set; }
	public Coordinate NextPosition { get; private set; }
	public Coordinate Goal { get; private set; }
	public IAbility Ability { get; private set; }

	private bool isMoving = false;
	private int prevStep, nextStep;

	private static System.Random rnd = new System.Random();
	
	// Convenience method to get the creature's definition
	public CreatureDefinition Definition
	{
		get { return CreatureDefinitions.ForType(creatureType); }
	}

	public ResourceCollection ToResources()
	{
		return ResourceCollection.FromMultiset(Definition.Recipe.MultisetSubtract(ResourceType.Energy, 1)).Add(health);
	}

	void Awake()
	{
		// Make sure we have the right ability attached
		if (Definition.Ability != null)
		{
			Ability = Definition.Ability.AddToCreature(this);
		}
	}

	void Start()
	{
		// Store our initial position
		Goal = NextPosition = Position = gameObject.Coordinate();
		prevStep = LevelManager.level.Steps;
		nextStep = prevStep + 4 - (int)Definition.Speed(this);
	}

	public void Update()
	{
		// Animate the creature moving
		var stepInterval = LevelManager.level.stepInterval;
		var cellSize = LevelManager.level.cellSize;

		var time = Time.timeSinceLevelLoad;
		var ratio = ((time / stepInterval) + 1 - prevStep) / (nextStep - prevStep);
		var direction = NextPosition - Position;
		var translation = new Vector3(direction.x, 0, direction.z) * ratio * cellSize;
		gameObject.SetPosition(Position);
		transform.position += translation;
	}

	public void Step()
	{
		if (LevelManager.level.Steps >= nextStep)
		{
			// TODO handle idle creatures
			prevStep = nextStep;
			nextStep += 4 - (int)Definition.Speed(this);
			if (!Definition.IsEnemy)
			{
				FriendlyStep();
			}
			else
			{
				EnemyStep();
			}

			// If the creature has a passive ability, do it
			if (HasAbility())
			{
				Ability.Passive();
			}
		}
	}

	private void EnemyStep()
	{
		Position = NextPosition;
		var possible = Position.CardinalNeighbors().Where(IsValidCoordinate).ToList();
		if (possible.Count > 0)
		{
			NextPosition = possible[rnd.Next(possible.Count)];
		}
		if (NextPosition != Position)
		{
			var direction = NextPosition - Position;
			transform.Rotate(new Vector3(0, AngleFor(direction)) - transform.rotation.eulerAngles);
		}
	}

	private void FriendlyStep()
	{
		Position = NextPosition;
		if (!Position.Equals(Goal) && health > 0)
		{
			NextPosition = NextCoordinate ();
			health -= 1;

			// Make our creature face the right direction
			if (NextPosition != Position)
			{
				var direction = NextPosition - Position;
				transform.Rotate(new Vector3(0, AngleFor(direction)) - transform.rotation.eulerAngles);
			}

			// Animate our creature if it has animation
			// TODO factor out animation into a UX class
			if (GetComponentInChildren<Animator>() && !isMoving)
			{
				isMoving = true;
				Debug.Log("Animating the creature moving.");
				GetComponentInChildren<Animator>().SetTrigger("StartMove");
			}
		}
		else
		{
			if (GetComponentInChildren<Animator>() && isMoving)
			{
				isMoving = false;
				Debug.Log("Animating the creature stopping.");
				GetComponentInChildren<Animator>().SetTrigger("StopMove");
			}
		}


		// If the creature loses all health, set it to an idle state
		if (health == 0)
		{
			Goal = Position;
		}
	}

	public void SetGoal(Coordinate coordinate)
	{
		Goal = coordinate;
	}

	// Returns true if this creature can reach the specified goal coordinate
	public bool CanReach(Coordinate goal)
	{
		var parents = DoBFS(Position, goal, IsValidCoordinate);
		return parents.Keys.Contains(goal);
	}

	public bool HasAbility()
	{
		return GetComponent<IAbility>() != null;
	}

	public void UseAbility(Coordinate coordinate)
	{
		if (!HasAbility())
		{
			throw new InvalidOperationException("This creature does not have an ability");
		}
		Ability.Use(coordinate);
	}

	private float AngleFor(Coordinate direction)
	{
		if (direction == Coordinate.up) { return -90; }
		if (direction == Coordinate.right) { return 0; }
		if (direction == Coordinate.down) { return 90; }
		if (direction == Coordinate.left) { return 180; }
		throw new ArgumentException("Given coordinate" + direction + " is not a direction", "direction");
	}

	private delegate bool CoordinatePredicate(Coordinate coordinate);
	private static IDictionary<Coordinate, Coordinate> DoBFS(Coordinate start, Coordinate end, CoordinatePredicate neighborPredicate)
	{
		// Initialize BFS data structures
		var distance = new Dictionary<Coordinate, int>();
		var parents = new Dictionary<Coordinate, Coordinate> ();
		var queue = new Queue<Coordinate> ();

		// Input our start coordinate
		distance [start] = 0;
		queue.Enqueue (start);

		while (queue.Count > 0)
		{
			var current = queue.Dequeue();
			var neighbors = current.CardinalNeighbors();
			foreach (Coordinate neighbor in neighbors)
			{
				if (!distance.ContainsKey(neighbor) && neighborPredicate(neighbor))
				{
					distance[neighbor] = distance[current] + 1;
					parents[neighbor] = current;
					queue.Enqueue(neighbor);
				}
			}
			// If we've already found the last coordinate, we do not need to keep searching.
			if (neighbors.Contains(end))
			{
				break;
			}
		}
		return parents;
	}

	// do a BFS and figure out the right path
	private Coordinate NextCoordinate()
	{
		// If the creature has no goal, just stick to the current position
		if (Goal == Position)
		{
			return Position;
		}

		var parents = DoBFS(Position, Goal, IsValidCoordinate);

		Coordinate next;
		// If we can reach the goal, then move the creature to the next step towards that goal
		if (parents.ContainsKey(Goal))
		{
			next = Goal;
			while (parents[next] != Position)
			{
				next = parents[next];
			}
			return next;
		}

		// Otherwise, do another BFS not accounting for terrain restrictions and try to move the creature there
		parents = DoBFS(Position, Goal, LevelManager.Terrain.Contains);
		// TODO assert that the level is fully connected
		next = Goal;
		while (parents[next] != Position)
		{
			next = parents[next];
		}
		return IsValidCoordinate(next) ? next : Position;
	}

	// Returns true if this creature is allowed to go to this coordinate
	private bool IsValidCoordinate(Coordinate coordinate)
	{
		return LevelManager.Terrain.Contains(coordinate)
			&& Definition.AllowedTerrain.Contains(LevelManager.Terrain[coordinate])
			&& !LevelManager.Creatures.CreatureList.Any(x => x != this && (x.Position == coordinate || x.NextPosition == coordinate));
	}

}
