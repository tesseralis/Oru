using UnityEngine;
using System;
using System.Collections;

public class ActionMarkers : MonoBehaviour
{

	private bool isEnabled = false;
	public bool IsActing { get; private set; }

	public void Enable(Creature creature)
	{
		isEnabled = true;
		IsActing = false;
		UXManager.Input.ActionButton += ToggleAbility;
	}

	public void Disable()
	{
		if (isEnabled && IsActing)
		{
			StopAbility();
		}
		isEnabled = false;
		IsActing = false;
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
		IsActing = true;
		gameObject.SetActive(true);
	}

	public void StopAbility()
	{
		if (!isEnabled)
		{
			throw new InvalidOperationException("Actions are not enabled.");
		}
		Debug.Log("Stopping creature ability.");
		IsActing = false;
		gameObject.SetActive(false);
	}

	public void ToggleAbility()
	{
		if (!IsActing)
		{
			StartAbility();
		}
		else
		{
			StopAbility();
		}
	}
}
