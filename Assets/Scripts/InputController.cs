using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// Defines handlers for input
public class InputController : MonoBehaviour
{
	private IDictionary<KeyCode, Action> keyEvents;

	public IDictionary<KeyCode, Action> Key { get { return keyEvents; } }

	void Awake ()
	{
		keyEvents = new Dictionary<KeyCode, Action>();
		// Initialize events we want to use
		foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
		{
			keyEvents[key] = null;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		foreach (KeyCode keycode in keyEvents.Keys)
		{
			if (Input.GetKey(keycode))
			{
				if (keyEvents[keycode] != null) { keyEvents[keycode](); }
			}
		}
	}
}
