using UnityEngine;
using System.Collections;

// Panel that shows when the game is paused
public class PausePanel : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		UXManager.Time.PauseToggled += gameObject.SetActive;
		gameObject.SetActive(false);
	}

}
