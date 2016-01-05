using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using Util;

// Defines handlers for input
public class InputController : MonoBehaviour
{
	private IDictionary<KeyCode, Action> keyEvents;
	private IDictionary<KeyCode, Action> keyDownEvents;

	public IDictionary<KeyCode, Action> Key { get { return keyEvents; } }
	public IDictionary<KeyCode, Action> KeyDown { get { return keyDownEvents; } }

	public Action<Coordinate> TerrainClicked;

	void Awake ()
	{
		keyEvents = new Dictionary<KeyCode, Action>();
		keyDownEvents = new Dictionary<KeyCode, Action>();
		// Initialize events we want to use
		foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
		{
			keyEvents[key] = null;
			keyDownEvents[key] = null;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
		{
			if (Input.GetKeyDown(keycode))
			{
				if (keyDownEvents[keycode] != null) { keyDownEvents[keycode](); }
			}
			if (Input.GetKey(keycode))
			{
				if (keyEvents[keycode] != null) { keyEvents[keycode](); }
			}
		}

		if (Input.GetButtonDown("Fire1"))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit) && !EventSystem.current.IsPointerOverGameObject())
			{
				// TODO I think I'll use (typesafe) tags instead
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
		if (Physics.Raycast(ray, out hit))
		{
			// TODO I think I'll use (typesafe) tags instead
			if (hit.transform.GetComponent<TerrainTile>())
			{
				return hit.transform.gameObject.Coordinate();
			}
		}
		return null;
	}
}
