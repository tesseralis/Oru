using UnityEditor;
using UnityEngine;
using System.Linq;

public class Level : EditorWindow
{
	int level;

	[MenuItem("Window/Level")]
	public static void  ShowWindow () {
		EditorWindow.GetWindow(typeof(Level));
	}

	public void OnGUI()
	{
		level = EditorGUILayout.Popup("Level", level, Deserializer.DeserializeLevelList().ToArray());
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
		Deserializer.DeserializeLevel(Deserializer.DeserializeLevelList()[level]);
		Debug.Log("Level Loaded");
	}

	public void SaveLevel()
	{
		// TODO Save the level
		Debug.Log("Level Saved");
	}
}