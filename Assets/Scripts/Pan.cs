﻿using UnityEngine;
using System.Collections;

public class Pan : MonoBehaviour
{

	public float translateFactor = 0.25f;

	// Use this for initialization
	void Start ()
	{
		// Add map controls
		GameManager.Input.Key[KeyCode.DownArrow] += () => DoPan(Vector2.down);
		GameManager.Input.Key[KeyCode.UpArrow] += () => DoPan(Vector2.up);
		GameManager.Input.Key[KeyCode.LeftArrow] += () => DoPan(Vector2.left);
		GameManager.Input.Key[KeyCode.RightArrow] += () => DoPan(Vector2.right);
	}

	// Pan our camera in the specified direction
	private void DoPan(Vector2 direction)
	{
		transform.Translate(new Vector3(direction.x, direction.y, direction.x) * translateFactor);
	}

}