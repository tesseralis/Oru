using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using Util;

// This class creates a popup over terrain blocks that have things in them
public class CoordinateInfo : MonoBehaviour
{

	public Text content;
	public ResourceList resourceList;

	void Awake ()
	{
		if (content == null) { content = GetComponentInChildren<Text>(); }
		if (resourceList == null) { resourceList = GetComponentInChildren<ResourceList>(); }
	}

	void Start()
	{
		gameObject.SetActive(false);

		LevelManager.Terrain.MouseEnterBlock += Show;
		LevelManager.Terrain.MouseExitBlock += x => Hide();
	}

	// Show the panel for the given coordinate
	void Show(Coordinate coordinate)
	{
		content.gameObject.SetActive(true);
		if (!LevelManager.Resources[coordinate].IsEmpty())
		{
			gameObject.SetActive(true);
			content.gameObject.SetActive(false);
			resourceList.ShowResources(LevelManager.Resources[coordinate]);
		}
		if (LevelManager.Recipes[coordinate] != null)
		{
			gameObject.SetActive(true);
			content.text = "?";
			resourceList.Hide();
		}

		if (LevelManager.Goals.goal.gameObject.Coordinate() == coordinate)
		{
			gameObject.SetActive(true);
			content.text = string.Format("Goal: {0} at this location.", LevelManager.Goals.goal.winningCreatureType);
			resourceList.Hide();
		}

		// Move ourselves to the new location
		gameObject.SetPosition(coordinate);

	}

	// Hide the panel
	void Hide()
	{
		gameObject.SetActive(false);
	}


}
