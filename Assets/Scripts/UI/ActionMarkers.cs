using UnityEngine;
using System;
using System.Collections;

public class ActionMarkers : MonoBehaviour {

	private bool isEnabled = false;
	private bool isActing = false;

	private ActionMarker[] markers;

	// Use this for initialization
	void Awake ()
	{
		if (markers == null)
		{
			markers = GetComponentsInChildren<ActionMarker>(true);
		}
	}

	public void Enable(Action<Coordinate> action)
	{
		isEnabled = true;
		GameManager.Input.OnSpace += ToggleAbility;
		foreach (var marker in markers)
		{
			marker.OnClick = coordinate => {
				action(coordinate);
				StopAbility();
			};
		}
	}

	public void Disable()
	{
		if (isEnabled && isActing)
		{
			StopAbility();
		}
		isEnabled = false;
		GameManager.Input.OnSpace -= ToggleAbility;
	}

	// The creature should start doing its ability
	public void StartAbility()
	{
		if (!isEnabled)
		{
			throw new InvalidOperationException("Actions are not enabled.");
		}
		Debug.Log("Engaging creature ability.");
		// TODO what to do if no creature selected?
		// stop the creature
		GetComponentInParent<Creature>().Goal = null;
		isActing = true;
		foreach (var marker in markers)
		{
			marker.gameObject.SetActive(true);
		}
	}

	public void StopAbility()
	{
		if (!isEnabled)
		{
			throw new InvalidOperationException("Actions are not enabled.");
		}
		Debug.Log("Stopping creature ability.");
		isActing = false;
		foreach (var marker in markers)
		{
			marker.gameObject.SetActive(false);
		}
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
