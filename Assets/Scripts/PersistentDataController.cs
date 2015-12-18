using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

public class PersistentDataController : MonoBehaviour {

	public static PersistentDataController controller;

	public string completionDataFile = "completionData.dat";

	// Privately represent the completion data as a list of completed levels
	private int[] completionData = new int[0];

	public bool GetCompletion(Scene level)
	{
		return completionData.Contains(level.buildIndex);
	}

	public void SetCompletion(Scene level, bool value)
	{
		var completionSet = new HashSet<int>(completionData);
		if (value)
		{
			completionSet.Add(level.buildIndex);
		}
		else
		{
			completionSet.Remove(level.buildIndex);
		}
		completionData = completionSet.ToArray();
	}

	// Make sure there's only one controller in a scene
	void Awake ()
	{
		if (controller == null)
		{
			DontDestroyOnLoad(gameObject);
			controller = this;
			Load();
		}
		else if (controller != this)
		{
			Destroy(gameObject);
		}
	}

	// Save our game data
	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file;
		if (!File.Exists(SaveFilePath()))
		{
			file = File.Create(SaveFilePath()); 
		}
		else
		{
			file = File.Open(SaveFilePath(), FileMode.Open);
		}

		bf.Serialize(file, completionData);
		file.Close();
	}

	// Load our game data
	public void Load()
	{
		if (File.Exists(SaveFilePath()))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(SaveFilePath(), FileMode.Open);
			completionData = (int[])bf.Deserialize(file);
			file.Close();
		}
	}

	private string SaveFilePath()
	{
		return string.Format("{0}/{1}", Application.persistentDataPath, completionDataFile);
	}
}
