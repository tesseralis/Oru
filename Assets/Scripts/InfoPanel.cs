using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InfoPanel : MonoBehaviour
{

	public Text nameDisplay;

	public Text descriptionDisplay;

	public string Name
	{
		set { nameDisplay.text = value; }
		get { return nameDisplay.text; }
	}

	public string Description
	{
		set { descriptionDisplay.text = value; }
		get { return descriptionDisplay.text; }
	}

}
