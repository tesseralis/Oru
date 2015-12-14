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

	public event Action<GameObject> Select;

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

	// Common actions when something is selected
	private void OnSelect(GameObject gameObject)
	{
		// Deselect the previous creature if any
		DeselectCreature();

		// Make this a child of the object
		transform.SetParent(gameObject.transform, false);

		// Make the entity marker visible
		if (entityMarker) { entityMarker.SetActive(true); }

		// Run the events in the handler
		if (Select != null) { Select(gameObject); }
	}

	private void DeselectCreature()
	{
		GameManager.Terrain.ClickBlock -= SetCurrentCreatureGoal;
		actionMarkers.OnStartAbility -= RemoveCurrentCreatureGoal;
		actionMarkers.Disable();
	}

	public void SelectGoal(Goal goal)
	{
		OnSelect(goal.gameObject);
	}

	public void SelectCreature(Creature creature)
	{
		OnSelect(creature.gameObject);

		GameManager.Terrain.ClickBlock += SetCurrentCreatureGoal;

		// Add a listener to the action markers
		if (creature.GetComponent<IAbility>() != null)
		{
			actionMarkers.Enable(creature.GetComponent<IAbility>().Use);
			actionMarkers.OnStartAbility += RemoveCurrentCreatureGoal;
		}
		else
		{
			actionMarkers.Disable();
		}
	}

	public void Deselect()
	{
		DeselectCreature();
		if (entityMarker) { entityMarker.SetActive(false); }
	}
		
	private void SetCurrentCreatureGoal(Coordinate coordinate)
	{
		var creature = GetComponentInParent<Creature>();
		creature.Goal = coordinate;
		actionMarkers.StopAbility();
	}

	private void RemoveCurrentCreatureGoal()
	{
		var creature = GetComponentInParent<Creature>();
		creature.Goal = null;
	}

}
