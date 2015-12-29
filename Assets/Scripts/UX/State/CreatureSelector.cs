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
	public event Action AbilityUsed;

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
		UXManager.Input.TerrainClicked -= OnClickBlock;
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

		UXManager.Input.TerrainClicked += OnClickBlock;

		// Add a listener to the action markers
		if (creature.HasAbility())
		{
			actionMarkers.Enable(creature);
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

	private void OnClickBlock(Coordinate coordinate)
	{
		if (actionMarkers.IsActing)
		{
			UseCreatureAbility(coordinate);
		}
		else
		{
			SetCurrentCreatureGoal(coordinate);
		}
	}

	private void UseCreatureAbility(Coordinate coordinate)
	{
		// TODO verify if we can use the ability here
		SelectedCreature.UseAbility(coordinate);
		actionMarkers.StopAbility();
		if (AbilityUsed != null) { AbilityUsed(); }
	}
		
	private void SetCurrentCreatureGoal(Coordinate coordinate)
	{
		if (SelectedCreature.CanReach(coordinate))
		{
			SelectedCreature.Goal = coordinate;
			if (GoalSet != null) { GoalSet(SelectedCreature, coordinate); }
		}
	}

	private void RemoveCurrentCreatureGoal()
	{
		SelectedCreature.Goal = null;
	}

}
