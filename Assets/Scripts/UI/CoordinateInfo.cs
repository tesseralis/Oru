using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using Util;

// This class creates a popup over terrain blocks that have things in them
public class CoordinateInfo : MonoBehaviour
{

	public Text content;

	void Awake ()
	{
		if (content == null) { content = GetComponentInChildren<Text>(); }
	}

	// Show the panel for the given coordinate
	public void Show(Coordinate coordinate)
	{
		if (!LevelController.Resources[coordinate].IsEmpty())
		{
			gameObject.SetActive(true);
			content.text = string.Join("\n",
				LevelController.Resources[coordinate].Select(e => e.Key + ": " + e.Value).ToArray());
		}
		if (LevelController.Recipes[coordinate] != null)
		{
			gameObject.SetActive(true);
			content.text = "?";
		}

		if (LevelController.gm.goal.gameObject.Coordinate() == coordinate)
		{
			gameObject.SetActive(true);
			content.text = string.Format("Goal: {0} at this location.", LevelController.gm.goal.winningCreatureType);
		}

		// Move ourselves to the new location
		gameObject.SetPosition(coordinate);

	}

	// Hide the panel
	public void Hide()
	{
		gameObject.SetActive(false);
	}


}
