using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using Util;

// Defines handlers for input
public class InputController : MonoBehaviour
{
	public Action<Coordinate> TerrainClicked;
	public Action ActionButton;
	public Action DeselectButton;

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown("Cancel"))
		{
			UXManager.Time.TogglePause();
		}
		// Disable all input if paused
		if (UXManager.Time.IsPaused)
		{
			return;
		}

		if (Input.GetButtonDown("Action"))
		{
			if (ActionButton != null) { ActionButton(); }
		}
		if (Input.GetButtonDown("Deselect"))
		{
			if (DeselectButton != null) { DeselectButton(); }
		}
		if (Input.GetButtonDown("Select"))
		{
			var currentCoordinate = CurrentCoordinate();
			if (currentCoordinate.HasValue)
			{
				TerrainClicked(currentCoordinate.Value);
			}
		}
	}

	// Return the current coordinate we are selecting, if any
	public Coordinate? CurrentCoordinate()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit) && !EventSystem.current.IsPointerOverGameObject())
		{
			if (hit.transform.GetComponent<TerrainTile>())
			{
				return hit.transform.gameObject.Coordinate();
			}
		}
		return null;
	}

	// Returns the input direction for panning
	public Vector2 PanDirection()
	{
		return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	}

	public float ZoomAmount()
	{
		return Input.GetAxis("Mouse ScrollWheel");
	}
}
