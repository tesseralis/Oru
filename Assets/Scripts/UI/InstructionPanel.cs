using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InstructionPanel : MonoBehaviour {

	public Text instructionText;

	// Use this for initialization
	void Start ()
	{
		// Show or hide instructions based on level text
		var instructions = LevelManager.level.instructions;
		if (!string.IsNullOrEmpty(instructions))
		{
			instructionText.text = instructions;
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
