using UnityEngine;
using System;
using System.Collections;

public class ActionMarkers : MonoBehaviour {

	private bool isEnabled = false;
	private bool isActing = false;

	private ActionMarker[] markers;

	public event Action OnStartAbility;
	public event Action OnStopAbility;
	public event Action AbilityUsed;

	public void Enable(Action<Coordinate> action)
	{
		if (markers == null)
		{
			markers = GetComponentsInChildren<ActionMarker>(true);
		}
		isEnabled = true;
		GameManager.Input.KeyDown[KeyCode.Space] += ToggleAbility;
		foreach (var marker in markers)
		{
			marker.OnClick = coordinate => {
				action(coordinate);
				if (AbilityUsed != null) { AbilityUsed(); }
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
		GameManager.Input.KeyDown[KeyCode.Space] -= ToggleAbility;
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
		if (OnStartAbility != null) { OnStartAbility(); }
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
		if (OnStopAbility != null) { OnStopAbility(); }
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
