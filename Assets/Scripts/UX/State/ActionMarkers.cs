using UnityEngine;
using System;
using System.Collections;

public class ActionMarkers : MonoBehaviour
{

	public bool isEnabled = false;
	public bool isActing = false;

	public void Enable(Creature creature)
	{
		if (!isEnabled) { UXManager.Input.ActionButton += ToggleAbility; }
		isEnabled = true;
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
		Debug.Log("Engaging creature ability.");
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
