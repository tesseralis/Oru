using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// The entity selector is responsible for the visual display of when something
/// is "selected" and acting.
/// </summary>
public class EntitySelector : MonoBehaviour
{
	public GameObject entityMarker;
	public GameObject actionMarkers;

	private bool isActing;

	// Use this for initialization
	void Start () 
	{
		if (entityMarker) { entityMarker.SetActive(false); }
		if (actionMarkers) { actionMarkers.SetActive(false); }

		GameManager.Creatures.OnSelect += SelectCreature;
		GameManager.gm.goal.OnClick += SelectGoal;
	}

	public void SelectGoal(Goal goal)
	{
		transform.SetParent(goal.transform, false);
		if (entityMarker) { entityMarker.SetActive(true); }

		GameManager.Terrain.OnClick -= SetCurrentCreatureGoal;
		GameManager.Input.OnSpace -= ToggleAbility;
	}

	public void SelectCreature(Creature creature)
	{
		// Make this creature our parent
		transform.SetParent(creature.transform, false);
		if (entityMarker) { entityMarker.SetActive(true); }

		GameManager.Terrain.OnClick += SetCurrentCreatureGoal;
		StopAbility();

		// Add a listener to the action markers
		if (creature.GetComponent<IAbility>() != null && actionMarkers)
		{
			foreach (var marker in actionMarkers.GetComponentsInChildren<ActionMarker>())
			{
				marker.OnClick = coordinate => 
				{
					creature.GetComponent<IAbility>().Use(coordinate);
					StopAbility();
				};
			}
		}

		// Disable creature creation once we click something
		UIManager.ui.creatureCreator.StopCreation();

		// Add keyboard shortcut for starting action
		GameManager.Input.OnSpace += ToggleAbility;
	}

	public void Deselect()
	{
		if (entityMarker) { entityMarker.SetActive(false); }
		if (actionMarkers) { actionMarkers.SetActive(false); }

		// remove the goal listener
		GameManager.Terrain.OnClick -= SetCurrentCreatureGoal;
		GameManager.Input.OnSpace -= ToggleAbility;
	}

	// The creature should start doing its ability
	public void StartAbility()
	{
		// TODO what to do if no creature selected?
		// stop the creature
		GetComponentInParent<Creature>().Goal = null;
		isActing = true;
		if (actionMarkers) { actionMarkers.SetActive(true); }
	}

	public void StopAbility()
	{
		// TODO what to do if no creature selected?
		isActing = false;
		if (actionMarkers) { actionMarkers.SetActive(false); }
	}

	public void ToggleAbility()
	{
		isActing = !isActing;
		if (isActing)
		{
			StartAbility();
		}
		else
		{
			StopAbility();
		}
	}

	private void SetCurrentCreatureGoal(Coordinate coordinate)
	{
		var creature = GetComponentInParent<Creature>();
		creature.Goal = coordinate;
		StopAbility();
	}

}
