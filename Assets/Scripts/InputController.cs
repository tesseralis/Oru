using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// Defines handlers for input
public class InputController : MonoBehaviour
{
	private IDictionary<KeyCode, Action> keyEvents;
	private IDictionary<KeyCode, Action> keyDownEvents;

	public IDictionary<KeyCode, Action> Key { get { return keyEvents; } }
	public IDictionary<KeyCode, Action> KeyDown { get { return keyDownEvents; } }

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
	}
}
