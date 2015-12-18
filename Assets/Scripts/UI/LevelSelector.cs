using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelSelector : MonoBehaviour {

	public void LoadLevel(string levelToLoad)
	{
		SceneManager.LoadScene(levelToLoad);
	}

}
