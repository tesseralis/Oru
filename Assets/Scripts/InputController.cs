using UnityEngine;
using System;
using System.Collections;

// Defines handlers for input
public class InputController : MonoBehaviour
{

	public Action OnSpace;

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (OnSpace != null) { OnSpace(); }
		}
	}
}
