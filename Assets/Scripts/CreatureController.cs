using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Util;

/// <summary>
/// Manages interactions between creatures and the rest of the system.
/// </summary>
public class CreatureController : MonoBehaviour
{
	// The game object to use to mark the currently selected creature
	public GameObject creatureMarker;
	// The game object used to show valid locations to do an action on
	public GameObject actionMarkers;

	public CreaturePrefabOptions creaturePrefabs;

	private Creature selectedCreature;

	// Tells us if we are acting right now.
	private bool isActing = false;

	public Action<Creature> OnSelect;

	public bool IsActing
	{
		get { return isActing; }
		set
		{
			isActing = value;
			if (actionMarkers) { actionMarkers.SetActive(isActing); }
		}
	}

	public IList<Creature> CreatureList
	{
		get
		{
			return new List<Creature>(GetComponentsInChildren<Creature>());
		}
	}

	public Creature SelectedCreature
	{
		get
		{
			return selectedCreature;
		}
		set
		{
			selectedCreature = value;
			// If creatures are unselected, get rid of the UI.
			if (selectedCreature == null)
			{
				if (creatureMarker) { creatureMarker.SetActive(false); }
				if (actionMarkers) { actionMarkers.SetActive(false); }
				return;
			}
			if (creatureMarker)
			{
				Debug.LogFormat("Setting creature marker of {0} to {1}", selectedCreature, creatureMarker);
				creatureMarker.SetActive(true);
				creatureMarker.transform.SetParent(value.gameObject.transform, false);
			}
			if (actionMarkers)
			{
				Debug.LogFormat("Setting action markers of {0} to {1}", selectedCreature, actionMarkers);
				actionMarkers.transform.SetParent(value.gameObject.transform, false);
				if (value.GetComponent<IAbility>() != null)
				{
					foreach (var marker in actionMarkers.GetComponentsInChildren<ActionMarker>())
					{
						marker.OnClick = value.GetComponent<IAbility>().Use;
					}
				}
			}
			IsActing = false;
			// Get rid of other UI
			GameManager.Recipes.IsCreating = false;

			// Trigger the UI changes
			if (OnSelect != null) { OnSelect(value); }
		}
	}

	// Returns true if we can add a creature at the given coordinate
	public bool CanCreateCreature(CreatureType creature, Coordinate coordinate)
	{
		var creatureDefinition = Creatures.ForType(creature);
		var resources = GameManager.Resources;
		var recipe = creatureDefinition.Recipe;
		// Figure out how many blocks we have available
		var availableResources = Neighbors(coordinate).Select(c => resources[c]);
		var resourceCount = Multiset.Empty<ResourceType>();
		foreach (var resource in availableResources)
		{
			resourceCount = resourceCount.MultisetAdd(resource);
		}
		return resourceCount.Contains(recipe)
			&& creatureDefinition.AllowedTerrain.Contains(GameManager.Terrain[coordinate]);

	}

	// Add a creature at a specified location if possible
	public void CreateCreature(CreatureType creature, Coordinate location)
	{
		if (!CanCreateCreature(creature, location))
		{
			throw new ArgumentException(string.Format("Cannot create {0} at {1}.",
				creature, location));
		}
		var resources = GameManager.Resources;
		this.AddChildWithComponent<Creature>(PrefabFor (creature), location);

		// Set this creature to be the new one

		// Remove the items from the neighboring coordinates.
		var neighbors = Neighbors(location);
		var remainder = Creatures.ForType(creature).Recipe;
		// Take items from the adjacent resources until we don't need any more.
		foreach (var neighbor in neighbors)
		{
			if (remainder.IsEmpty())
			{
				break;
			}
			var difference = GameManager.Resources[neighbor].MultisetSubtract(remainder);
			remainder = remainder.MultisetSubtract(resources[neighbor]);

			resources[neighbor] = difference;
		}

	}

	private void SetSelectedCreatureGoal(Coordinate goal)
	{
		if (SelectedCreature)
		{
			SelectedCreature.Goal = goal;
			IsActing = false;
		}
	}

	public void Start()
	{
		// Hook up the event handler
		// TODO Change the code to add and remove these event handlers based on state.
		GameManager.Terrain.OnClick += SetSelectedCreatureGoal;
	}

	/// <summary>
	/// Move all the creatures forward one game step.
	/// </summary>
	public void Step()
	{
		foreach(Creature creature in CreatureList)
		{
			creature.Step();
		}
	}

	public void Update()
	{
		if (Input.GetKeyDown("space") && (SelectedCreature != null))
		{
			Debug.LogFormat("Detected a space on {0}", SelectedCreature);
			// If the creature is moving, make it stop
			SelectedCreature.Goal = null;
			// Toggle the action marker
			IsActing = !IsActing;
		}
	}

	GameObject PrefabFor (CreatureType creature)
	{
		switch (creature) {
		case CreatureType.Duck:
			return creaturePrefabs.duckPrefab;
		case CreatureType.Dragon:
			return creaturePrefabs.dragonPrefab;
		default:
			throw new ArgumentException("Illegal creature type", creature.ToString());
		}
	}

	private static IList<Coordinate> Neighbors(Coordinate coordinate)
	{
		int[] range = {-1, 0, 1};
		return (from x in range
			from z in range
			select coordinate + new Coordinate(x, z)).ToList();

	}
}

[Serializable]
public class CreaturePrefabOptions
{
	public GameObject duckPrefab;
	public GameObject dragonPrefab;
}