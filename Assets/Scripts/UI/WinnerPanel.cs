using UnityEngine;
using System.Collections;

public class WinnerPanel : MonoBehaviour
{
	void Start ()
	{
		// Start creating the recipe
		LevelManager.Goals.OnWin += DisplayWinInfo;
		gameObject.SetActive(false);
	}

	void DisplayWinInfo()
	{
		gameObject.SetActive(true);
	}
}
