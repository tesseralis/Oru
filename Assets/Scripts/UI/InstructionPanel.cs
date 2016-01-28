using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InstructionPanel : MonoBehaviour {

	public Text instructionText;

	void Awake()
	{
		LevelManager.LevelLoaded += OnLevelLoaded;
	}

	// Use this for initialization
	void OnLevelLoaded (LevelManager level)
	{
		// Show or hide instructions based on level text
		var instructions = LevelManager.level.instructions;
		if (!string.IsNullOrEmpty(instructions))
		{
			instructionText.text = instructions.Replace("\n", "\n\n");
			ShowInstructions();
		}
		else
		{
			HideInstructions();
		}
	}

	public void ShowInstructions()
	{
		gameObject.SetActive(true);
	}

	public void HideInstructions()
	{
		gameObject.SetActive(false);
	}
}
