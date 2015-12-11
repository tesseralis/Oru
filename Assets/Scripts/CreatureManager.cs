using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages interactions between creatures and the rest of the system.
/// </summary>
public class CreatureManager : MonoBehaviour
{
	// The game object to use to mark the currently selected creature
	public GameObject creatureMarker;
	// The game object used to show valid locations to do an action on
	public GameObject actionMarkers;
	// TODO abstract out UI and game logic
	// The panel to show information about the creature
	public InfoPanel infoPanel;

	public CreaturePrefabOptions creaturePrefabs;

	private Creature selectedCreature;

	// Tells us if we are acting right now.
	private bool isActing = false;

	public bool IsActing
	{
		get { return isActing; }
		set
		{
			isActing = value;
			if (actionMarkers) { actionMarkers.SetActive(isActing); }
		}
	}

	public IList<Creature> Creatures
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
				foreach (var marker in actionMarkers.GetComponentsInChildren<ActionMarker>())
				{
					marker.Action = value.GetComponent<IAction>();
				}
			}
			IsActing = false;
			// Update the info panel
			infoPanel.Name = value.creatureType.ToString();
			infoPanel.Description = "Action: " + value.GetComponent<IAction>();
			// Get rid of other UI
			GameManager.gm.recipes.IsCreating = false;
		}
	}

	// Add a creature at a specified location
	public void AddCreature(CreatureType creature, Coordinate location)
	{
		GameObject creaturePrefab;
		switch(creature)
		{
		case CreatureType.Duck:
			creaturePrefab = creaturePrefabs.duckPrefab;
			break;
		case CreatureType.Dragon:
		default:
			creaturePrefab = creaturePrefabs.dragonPrefab;
			break;
		}
		GameObject newCreature = Instantiate(creaturePrefab);
		GameManager.gm.SetPosition(newCreature, location);
		newCreature.transform.SetParent(this.transform);

		// Set this creature to be the new one
	}

	public void SetSelectedCreatureGoal(Coordinate goal)
	{
		if (SelectedCreature)
		{
			SelectedCreature.Goal = goal;
			IsActing = false;
		}
	}

	/// <summary>
	/// Move all the creatures forward one game step.
	/// </summary>
	public void Step()
	{
		foreach(Creature creature in Creatures)
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
}

[Serializable]
public class CreaturePrefabOptions
{
	public GameObject duckPrefab;
	public GameObject dragonPrefab;
}