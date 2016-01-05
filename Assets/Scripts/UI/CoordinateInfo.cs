using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using Util;

// This class creates a popup over terrain blocks that have things in them
public class CoordinateInfo : MonoBehaviour
{
	public GameObject panel;
	public Text content;
	public ResourceList resourceList;

	void Awake ()
	{
		if (content == null) { content = GetComponentInChildren<Text>(); }
		if (resourceList == null) { resourceList = GetComponentInChildren<ResourceList>(); }
	}

	void Update()
	{
		Coordinate? hit = UXManager.Input.CurrentCoordinate();
		if (hit.HasValue)
		{
			Show(hit.Value);
		}
		else
		{
			Hide();
		}
	}

	// Show the panel for the given coordinate
	void Show(Coordinate coordinate)
	{
		content.gameObject.SetActive(true);
		panel.SetActive(false);
		if (!LevelManager.Resources[coordinate].IsEmpty())
		{
			panel.SetActive(true);
			content.gameObject.SetActive(false);
			resourceList.ShowResources(LevelManager.Resources[coordinate]);
		}
		if (LevelManager.Recipes[coordinate] != null)
		{
			panel.SetActive(true);
			content.text = "?";
			resourceList.Hide();
		}

		if (LevelManager.Goals.goal.gameObject.Coordinate() == coordinate)
		{
			panel.SetActive(true);
			content.text = string.Format("Goal: {0} at this location.", LevelManager.Goals.goal.winningCreatureType);
			resourceList.Hide();
		}

		// Move ourselves to the new location
		gameObject.SetPosition(coordinate);

	}

	// Hide the panel
	void Hide()
	{
		panel.SetActive(false);
	}

}
