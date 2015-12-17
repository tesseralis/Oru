using UnityEngine;
using System;
using System.Collections;

public class ActionMarkers : MonoBehaviour {

	private bool isEnabled = false;
	private bool isActing = false;

	private ActionMarker[] markers;

	public event Action OnStartAbility;
	public event Action OnStopAbility;

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
		GameManager.Input.Key[KeyCode.Space] += ToggleAbility;
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
		GameManager.Input.Key[KeyCode.Space] -= ToggleAbility;
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
		foreach (var marker in markers)
		{
			marker.gameObject.SetActive(true);
		}
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
		foreach (var marker in markers)
		{
			marker.gameObject.SetActive(false);
		}
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
