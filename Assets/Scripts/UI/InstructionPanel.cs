using UnityEngine;
using System.Collections;

public class InstructionPanel : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		// TODO Show or hide instructions based on level text
	}

	public void HideInstructions()
	{
		gameObject.SetActive(false);
	}
}
