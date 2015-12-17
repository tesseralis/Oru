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
		if (!GameManager.Resources[coordinate].IsEmpty())
		{
			gameObject.SetActive(true);
			content.text = string.Join("\n",
				GameManager.Resources[coordinate].Select(e => e.Key + ": " + e.Value).ToArray());
		}
		if (GameManager.Recipes[coordinate] != null)
		{
			gameObject.SetActive(true);
			content.text = "?";
		}

		if (GameManager.gm.goal.Coordinate() == coordinate)
		{
			gameObject.SetActive(true);
			content.text = string.Format("Goal: {0} at this location.", GameManager.gm.goal.winningCreatureType);
		}

		// Move ourselves to the new location
		GameManager.gm.SetPosition(gameObject, coordinate);

	}

	// Hide the panel
	public void Hide()
	{
		gameObject.SetActive(false);
	}


}
