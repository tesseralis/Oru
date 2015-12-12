using UnityEngine;
using System.Collections;

// Main controller for all UI.
public class UIManager : MonoBehaviour {

	public WinnerPanel winnerPanel;

	// Use this for initialization
	void Start () {
		if (winnerPanel == null)
		{
			winnerPanel = GetComponentInChildren<WinnerPanel>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		winnerPanel.gameObject.SetActive(GameManager.gm.HasWon);
	}
}
