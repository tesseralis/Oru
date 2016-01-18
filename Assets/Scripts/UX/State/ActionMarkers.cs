using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using Util;

public class ActionMarkers : MonoBehaviour
{

	public bool isEnabled = false;
	public bool isActing = false;

	private Creature creature;

	public GameObject actionMarkerPositive;
	public GameObject actionMarkerNegative;

	void Update()
	{
		if (isActing)
		{
			gameObject.DestroyAllChildren();
			foreach (var coordinate in creature.Position.CardinalNeighbors())
			{
				if (creature.Ability.CanUse(coordinate))
				{
					gameObject.AddChild(actionMarkerPositive, coordinate);
				}
				else
				{
					gameObject.AddChild(actionMarkerNegative, coordinate);
				}
			}
		}
	}

	public void Enable(Creature creature)
	{	
		if (!isEnabled) { UXManager.Input.ActionButton += ToggleAbility; }
		isEnabled = true;
		this.creature = creature;
		StopAbility();
	}

	public void Disable()
	{
		if (isEnabled && isActing)
		{
			StopAbility();
		}
		isEnabled = false;
		isActing = false;
		UXManager.Input.ActionButton -= ToggleAbility;
	}

	// The creature should start doing its ability
	public void StartAbility()
	{
		if (!isEnabled)
		{
			throw new InvalidOperationException("Actions are not enabled.");
		}
		Debug.Log("Starting creature ability.");


		isActing = true;
		gameObject.SetActive(true);
	}

	public void StopAbility()
	{
		if (!isEnabled)
		{
			throw new InvalidOperationException("Actions are not enabled.");
		}
		Debug.Log("Stopping creature ability.");
		isActing = false;
		gameObject.SetActive(false);
	}

	public void ToggleAbility()
	{
		if (!isActing)
		{
			StartAbility();
		}
		else
		{
			StopAbility();
		}
	}
}
