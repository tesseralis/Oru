using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;

public class Level : EditorWindow
{
	int level;

	private string[] LevelList
	{
		get { return Serialization.DeserializeLevelList().ToArray(); }
	}

	[MenuItem("Window/Level")]
	public static void  ShowWindow () {
		EditorWindow.GetWindow(typeof(Level));
	}

	public void OnGUI()
	{
		level = EditorGUILayout.Popup("Level", level, LevelList);
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
		Serialization.DeserializeLevel(LevelList[level]);
		Debug.Log("Level Loaded");
	}

	public void SaveLevel()
	{
		var levelFile = UnityEngine.Resources.Load<TextAsset>("Levels/" + LevelList[level]);
		var filePath = AssetDatabase.GetAssetPath(levelFile);
		var writer = new StreamWriter(filePath);
		Debug.Log(Serialization.SerializeLevel(writer));
		writer.Close();
		Debug.Log("Level Saved");
	}
}
