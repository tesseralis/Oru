using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
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

	void Awake()
	{
		var rectTransform = buttonContainer.GetComponent<RectTransform>();
		var anchoredPosition = rectTransform.anchoredPosition;
		rectTransform.anchoredPosition = new Vector2(anchoredPosition.x, 0);
	}

	// TODO this can be done at initialization since the number of levels doesn't change
	void Update()
	{
		gameObject.SetActive(true);
		buttonContainer.DestroyAllChildren();

		var levels = GameManager.game.Levels;

		// Populate the UI with buttons corresponding with the recipes
		for (int i = 0; i < levels.Count; i++)
		{
			var level = levels[i];
			DelegateButton newButton = Instantiate(buttonPrefab);
			newButton.transform.SetParent(buttonContainer.transform, false);
			newButton.GetComponentInChildren<Text>().text = "Level " + (i+1);
			// Translate the new object by the index amount
			var transform = newButton.GetComponent<RectTransform>();
			transform.anchoredPosition += Vector2.down * i * (transform.rect.height + buttonSpacing);

			// Set the component to do the hooked in action when clicked
			var createButton = newButton.GetComponent<DelegateButton>();
			createButton.Click += () => GameManager.game.LoadLevel(level);

			if (GameManager.game.GetCompletion(level))
			{
				newButton.image.color = finishedColor;
			}
			else
			{
				newButton.image.color = unfinishedColor;	
			}
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
