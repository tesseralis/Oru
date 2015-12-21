using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelSelector : MonoBehaviour {

	// TODO Make the level selector more autonomous
	public Button[] levelButtons;
	public string[] levels;
	public Sprite unfinishedSprite;
	public Sprite finishedSprite;

	void Start()
	{
		GameController.controller.Load();
		foreach (string level in levels)
		{
			Debug.LogFormat("{0}: {1}", level, GameController.controller.GetCompletion(level));
		}

	}

	void Update()
	{
		for (int i = 0; i < levels.Length; i++)
		{
			var button = levelButtons[i];
			var level = levels[i];
			if (GameController.controller.GetCompletion(level))
			{
				button.image.sprite = finishedSprite;
			}
			else
			{
				button.image.sprite = unfinishedSprite;	
			}
		}
	}

	public void LoadLevel(string levelToLoad)
	{
		SceneManager.LoadScene(levelToLoad);
	}

	public void ClearProgress()
	{
		foreach (string level in levels)
		{
			GameController.controller.SetCompletion(level, false);
		}
		GameController.controller.Save();
	}

}
