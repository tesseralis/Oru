using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;

// Event handlers for the button that can be clicked to create creatures
// TODO this is so generic--could I factor this out to my event framework?
public class CreationButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{

	public event Action Click;
	public event Action MouseEnter;
	public event Action MouseExit;

	public void OnPointerDown(PointerEventData eventData)
	{
		if (Click != null) { Click(); }
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (MouseEnter != null) { MouseEnter(); }
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (MouseExit != null) { MouseExit(); }
	}

}
