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
	public Action<Creature> CreatureClicked;
	public Action ActionButton;

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown("Jump"))
		{
			if (ActionButton != null) { ActionButton(); }
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
				// TODO make it so you figure out if there is a creature on the tile instead
				if (hit.transform.GetComponent<Creature>() && CreatureClicked != null)
				{
					CreatureClicked(hit.transform.GetComponent<Creature>());
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
