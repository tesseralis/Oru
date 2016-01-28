using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

// A button that can receive delegates
public class DelegateButton : MonoBehaviour
{

	public event Action Click;
	public event Action MouseEnter;
	public event Action MouseExit;

	public void OnClick()
	{
		if (Click != null) { Click(); }
	}
	public void OnMouseEnter()
	{
		if (MouseEnter != null) { MouseEnter(); }
	}

	public void OnMouseExit()
	{
		if (MouseExit != null) { MouseExit(); }
	}

}
