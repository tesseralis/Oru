using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

// A button that can receive delegates
public class DelegateButton : Button
{

	public event Action Click;
	public event Action MouseEnter;
	public event Action MouseExit;

	public override void OnPointerDown(PointerEventData eventData)
	{
		if (Click != null) { Click(); }
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		if (MouseEnter != null) { MouseEnter(); }
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		if (MouseExit != null) { MouseExit(); }
	}

}
