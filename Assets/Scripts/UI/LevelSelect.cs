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

	private IList<Button> buttons = new List<Button>();

	void Awake()
	{
		var rectTransform = buttonContainer.GetComponent<RectTransform>();
		var anchoredPosition = rectTransform.anchoredPosition;
		rectTransform.anchoredPosition = new Vector2(anchoredPosition.x, 0);
	}

	void Start()
	{
		var levels = GameManager.game.Levels;
		var buttonHeight = buttonPrefab.GetComponent<RectTransform>().rect.height;
		for (int i = 0; i < levels.Count; i++)
		{
			var level = levels[i];
			DelegateButton newButton = Instantiate(buttonPrefab);
			newButton.transform.SetParent(buttonContainer.transform, false);
			newButton.GetComponentInChildren<Text>().text = "Level " + (i+1);
			// Translate the new object by the index amount
			var buttonTransform = newButton.GetComponent<RectTransform>();
			buttonTransform.anchoredPosition += Vector2.down * i * (buttonHeight + buttonSpacing);

			// Set the component to do the hooked in action when clicked
			newButton.Click += () => GameManager.game.LoadLevel(level);

			// Add to the list of buttons
			buttons.Add(newButton);
		}
		var containerTransform = buttonContainer.GetComponent<RectTransform>();
		var size = containerTransform.sizeDelta;
		containerTransform.sizeDelta = new Vector2(size.x, levels.Count * (buttonHeight + buttonSpacing) + buttonSpacing);

	}

	void Update()
	{
		var levels = GameManager.game.Levels;

		// Populate the UI with buttons corresponding with the recipes
		for (int i = 0; i < levels.Count; i++)
		{
			var level = levels[i];
			var button = buttons[i];

			button.image.color = GameManager.game.GetCompletion(level) ? finishedColor : unfinishedColor;
		}
	}

	// Clear our game progress
	public void ClearProgress()
	{
		foreach (string level in GameManager.game.Levels)
		{
			GameManager.game.SetCompletion(level, false);
		}
		GameManager.game.Save();
	}

}
