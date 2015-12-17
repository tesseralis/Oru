using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelSelector : MonoBehaviour {
	
	public string levelToLoad = "LevelPrototype";

	public void LoadLevel()
	{
		SceneManager.LoadScene(levelToLoad);
	}

}
