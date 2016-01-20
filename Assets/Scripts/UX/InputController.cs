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
	public Action CancelButton;

	// Update is called once per frame
	void Update ()
	{
		// TODO pick a different key for pausing (escape?)
		// TODO add a pausing button
		if (Input.GetKeyDown(KeyCode.P))
		{
			UXManager.Time.TogglePause();
		}
		// Disable all input if paused
		if (UXManager.Time.IsPaused)
		{
			return;
		}

		if (Input.GetButtonDown("Jump"))
		{
			if (ActionButton != null) { ActionButton(); }
		}
		if (Input.GetButtonDown("Cancel"))
		{
			if (CancelButton != null) { CancelButton(); }
		}
		if (Input.GetButtonDown("Fire1"))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit) && !EventSystem.current.IsPointerOverGameObject())
			{
				if (hit.transform.GetComponent<TerrainTile>() && TerrainClicked != null)
				{
					TerrainClicked(hit.transform.gameObject.Coordinate());
				}
			}
		}
	}

	// Return the current coordinate we are selecting, if any
	public Coordinate? CurrentCoordinate()
	{
		// TODO refactor with the other method
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		// TODO Make it so that this is disabled when another UI element is on top
		// without having a weird wobble effect with the coordinate grid
		if (Physics.Raycast(ray, out hit))
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
}
