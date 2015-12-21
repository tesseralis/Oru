using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// The entity selector is responsible for the visual display of when something
/// is "selected" and acting.
/// </summary>
public class CreatureSelector : MonoBehaviour
{
	public GameObject creatureMarker;
	public ActionMarkers actionMarkers;

	public event Action<Creature> Selected;
	public event Action Deselected;
	public event Action<Creature, Coordinate> GoalSet;

	private bool isActing;

	public Creature SelectedCreature { get; private set; }

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
		if (creatureMarker) { creatureMarker.SetActive(false); }
		actionMarkers.Disable();
		LevelManager.Creatures.CreatureSelected += SelectCreature;
		LevelManager.Creatures.CreatureDestroyed += (x) => Deselect();
	}

	private void DeselectCreature()
	{
		LevelManager.Terrain.ClickBlock -= SetCurrentCreatureGoal;
		actionMarkers.OnStartAbility -= RemoveCurrentCreatureGoal;
		actionMarkers.Disable();
		SelectedCreature = null;
		transform.SetParent(null, false);
	}

	public void SelectCreature(Creature creature)
	{
		// Deselect the previous creature if any
		DeselectCreature();

		SelectedCreature = creature;

		// Make this a child of the object
		transform.SetParent(creature.transform, false);

		// Make the entity marker visible
		if (creatureMarker) { creatureMarker.SetActive(true); }

		LevelManager.Terrain.ClickBlock += SetCurrentCreatureGoal;

		// Add a listener to the action markers
		if (creature.HasAbility())
		{
			actionMarkers.Enable(creature.Ability.Use);
			actionMarkers.OnStartAbility += RemoveCurrentCreatureGoal;
		}
		else
		{
			actionMarkers.Disable();
		}

		// Run the events in the handler
		if (Selected != null) { Selected(creature); }
	}

	public void DestroySelectedCreature()
	{
		LevelManager.Creatures.DestroyCreature(SelectedCreature);
	}

	public void Deselect()
	{
		DeselectCreature();
		if (creatureMarker) { creatureMarker.SetActive(false); }

		// Run the events in the handler
		if (Deselected != null) { Deselected(); }
	}
		
	private void SetCurrentCreatureGoal(Coordinate coordinate)
	{
		if (SelectedCreature.CanReach(coordinate))
		{
			SelectedCreature.Goal = coordinate;
			if (GoalSet != null) { GoalSet(SelectedCreature, coordinate); }
		}
		if (SelectedCreature.HasAbility()) { actionMarkers.StopAbility(); }
	}

	private void RemoveCurrentCreatureGoal()
	{
		SelectedCreature.Goal = null;
	}

}
