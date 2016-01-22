using UnityEditor;
using UnityEngine;
using System.Linq;

public class Level : EditorWindow
{
	[MenuItem("Window/Level")]
	public static void  ShowWindow () {
		EditorWindow.GetWindow(typeof(Level));
	}

	public void OnGUI()
	{
		GUILayout.Label ("Level Settings", EditorStyles.boldLabel);
		if (GUILayout.Button("Load"))
		{
			LoadLevel();
		}
		if (GUILayout.Button("Save"))
		{
			SaveLevel();
		}
	}

	public void LoadLevel()
	{
		// TODO make the deserializer work
		// TODO be able to load all the level
		Deserializer.DeserializeLevel("crane");
		Debug.Log("Level Loaded");
	}

	public void SaveLevel()
	{
		// TODO Save the level
		Debug.Log("Level Saved");
	}
}