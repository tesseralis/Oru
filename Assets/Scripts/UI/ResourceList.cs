using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Util;

public class ResourceList : MonoBehaviour
{

	public GameObject resourceCountPanelPrefab;
	public float resourceSpacing = 30f;

	// TODO a lot of this can be factored with the methods in RecipeList
	public void SetContents(IDictionary<ResourceType, int> recipe)
	{
		// Delete all the children
		gameObject.DestroyAllChildren();

		int i = 0;
		foreach(var entry in recipe)
		{
			GameObject newResource = Instantiate(resourceCountPanelPrefab);
			newResource.transform.SetParent(gameObject.transform, false);

			// Translate the new object by the index amount
			var transform = newResource.GetComponent<RectTransform>();
			transform.anchoredPosition += Vector2.down * i * resourceSpacing;
			newResource.GetComponent<ResourceCountPanel>().SetContents(entry.Key, entry.Value);
			i++;
		}
	}

}
