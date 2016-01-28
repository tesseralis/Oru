using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Util;

// TODO refactor with the recipe list
public class LevelSelect : MonoBehaviour
{
	public GameObject buttonContainer;
	public DelegateButton buttonPrefab;

	public Color finishedColor;
	public Color unfinishedColor;

	public float buttonSpacing = 5.0f;
	// TODO This should be managed somewhere other than the UI (probably)
	public bool unlockAllLevels = false;

//	private IList<Button> buttons = new List<Button>();
//
//	void Awake()
//	{
//		var rectTransform = buttonContainer.GetComponent<RectTransform>();
//		var anchoredPosition = rectTransform.anchoredPosition;
//		rectTransform.anchoredPosition = new Vector2(anchoredPosition.x, 0);
//	}
//
//	void Start()
//	{
//		var levels = GameManager.game.Levels;
//		var buttonHeight = buttonPrefab.GetComponent<RectTransform>().rect.height;
//		for (int i = 0; i < levels.Count; i++)
//		{
//			var level = levels[i];
//			DelegateButton newButton = Instantiate(buttonPrefab);
//			newButton.transform.SetParent(buttonContainer.transform, false);
//			newButton.GetComponentInChildren<Text>().text = "Level " + (i+1);
//			// Translate the new object by the index amount
//			var buttonTransform = newButton.GetComponent<RectTransform>();
//			buttonTransform.anchoredPosition += Vector2.down * i * (buttonHeight + buttonSpacing);
//
//			// Set the component to do the hooked in action when clicked
//			newButton.Click += () => GameManager.game.LoadLevel(level);
//
//			// Add to the list of buttons
//			buttons.Add(newButton);
//		}
//		var containerTransform = buttonContainer.GetComponent<RectTransform>();
//		var size = containerTransform.sizeDelta;
//		containerTransform.sizeDelta = new Vector2(size.x, levels.Count * (buttonHeight + buttonSpacing) + buttonSpacing);
//
//	}
//
//	void Update()
//	{
//		var levels = GameManager.game.Levels;
//
//		// Only go up to the highest completed level;
//		var maxLevelComplete = -1;
//		for (int i = 0; i < levels.Count; i++)
//		{
//			if (GameManager.game.GetCompletion(levels[i]))
//			{
//				maxLevelComplete = i;
//			}
//		}
//
//		// Populate the UI with buttons corresponding with the levels
//		for (int i = 0; i < levels.Count; i++)
//		{
//			var level = levels[i];
//			var button = buttons[i];
//
//			button.image.color = GameManager.game.GetCompletion(level) ? finishedColor : unfinishedColor;
//
//			// Only show levels we've unlocked so far
//			if (unlockAllLevels || i <= maxLevelComplete + 1)
//			{
//				button.gameObject.SetActive(true);
//			}
//			else
//			{
//				button.gameObject.SetActive(false);
//			}
//		}
//	}

	// Clear our game progress
	public void ClearProgress()
	{
		foreach (string level in GameManager.game.Levels)
		{
			GameManager.game.SetCompletion(level, false);
		}
		GameManager.game.Save();

		// Reload the level
	}

}
