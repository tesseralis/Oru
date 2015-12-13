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
	public ActionMarkers actionMarkers;

	private bool isActing;

	void Awake ()
	{
		if (actionMarkers == null)
		{
			actionMarkers = GetComponentInChildren<ActionMarkers>();
		}
	}

	// Use this for initialization
	void Start () 
	{
		if (entityMarker) { entityMarker.SetActive(false); }
		actionMarkers.Disable();
		GameManager.Creatures.OnSelect += SelectCreature;
		GameManager.gm.goal.OnClick += SelectGoal;
	}

	public void SelectGoal(Goal goal)
	{
		transform.SetParent(goal.transform, false);
		if (entityMarker) { entityMarker.SetActive(true); }

		GameManager.Terrain.OnClick -= SetCurrentCreatureGoal;
		actionMarkers.Disable();
	}

	public void SelectCreature(Creature creature)
	{
		actionMarkers.Disable();
		// Make this creature our parent
		transform.SetParent(creature.transform, false);
		if (entityMarker) { entityMarker.SetActive(true); }

		GameManager.Terrain.OnClick += SetCurrentCreatureGoal;

		// Add a listener to the action markers
		if (creature.GetComponent<IAbility>() != null)
		{
			actionMarkers.Enable(creature.GetComponent<IAbility>().Use);
		}

		// Disable creature creation once we click something
		UIManager.ui.creatureCreator.StopCreation();
	}

	public void Deselect()
	{
		if (entityMarker) { entityMarker.SetActive(false); }

		actionMarkers.Disable();

		// remove the goal listener
		GameManager.Terrain.OnClick -= SetCurrentCreatureGoal;
	}
		
	private void SetCurrentCreatureGoal(Coordinate coordinate)
	{
		var creature = GetComponentInParent<Creature>();
		creature.Goal = coordinate;
		actionMarkers.StopAbility();
	}

}
