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

	private Action<Coordinate> moveCreatureAction;

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

		GameManager.Terrain.OnClick -= moveCreatureAction;
		moveCreatureAction = null;
	}

	public void SelectCreature(Creature creature)
	{
		// Make this creature our parent
		transform.SetParent(creature.transform, false);
		if (entityMarker) { entityMarker.SetActive(true); }

		GameManager.Terrain.OnClick -= moveCreatureAction;
		// Add a listener to move this creature
		moveCreatureAction = coordinate => {
			creature.Goal = coordinate;
			StopAbility(); // TODO should this be a different listener?
		};
		GameManager.Terrain.OnClick += moveCreatureAction;

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
	}

	public void Deselect()
	{
		if (entityMarker) { entityMarker.SetActive(false); }
		if (actionMarkers) { actionMarkers.SetActive(false); }

		// remove the goal listener
		GameManager.Terrain.OnClick -= moveCreatureAction;
		moveCreatureAction = null;
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

	// TODO move this to a generic key input controller
	private bool isActing = false;
	public void Update()
	{
		if (Input.GetKeyDown("space"))
		{
			ToggleAbility();
		}
	}
}
