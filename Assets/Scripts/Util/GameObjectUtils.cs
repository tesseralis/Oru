#if UNITY_EDITOR
	using UnityEditor;
#endif
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Util
{

	// Defines extension methods related to coordinates
	public static class GameObjectUtils
	{
		// Store the cell size for reference
		private static float cellSize = LevelManager.cellSize;

		public static Coordinate Coordinate(this GameObject gameObject)
		{
			var position = gameObject.gameObject.transform.position;
			return new Coordinate(Mathf.RoundToInt(position.x / cellSize), Mathf.RoundToInt(position.z / cellSize));
		}

		public static void SetPosition(this GameObject gameObject, Coordinate coordinate)
		{
			gameObject.transform.position = new Vector3(coordinate.x * cellSize, gameObject.transform.position.y, coordinate.z * cellSize);
		}

		/// <summary>
		/// Add a child object to the script, using the given prefab.
		/// </summary>
		/// <returns>The child GameObject.</returns>
		public static GameObject AddChild(this GameObject gameObject, GameObject prefab)
		{
			#if UNITY_EDITOR
			GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
			#else
			GameObject newObject = (GameObject)GameObject.Instantiate(prefab);
			#endif
			newObject.transform.SetParent(gameObject.transform, false);
			return newObject;
		}

		/// <summary>
		/// Add a child object to the script, using the given prefab.
		/// </summary>
		/// <returns>The child GameObject.</returns>
		public static GameObject AddChild(this GameObject gameObject, GameObject prefab, Vector3 position)
		{
			var child = gameObject.AddChild(prefab);
			child.transform.position = position;
			return child;
		}

		/// <summary>
		/// Add a child object to the script, using the given prefab.
		/// </summary>
		/// <returns>The child GameObject.</returns>
		public static GameObject AddChild(this GameObject gameObject, GameObject prefab, Coordinate coordinate)
		{
			return gameObject.AddChild(prefab, new Vector3(coordinate.x * cellSize, 0, coordinate.z * cellSize));
		}

		/// <summary>
		/// Add a child object to the script with the given component, using the given prefab.
		/// </summary>
		/// <returns>The component belonging to the child that was added.</returns>
		/// <typeparam name="T">The type of component to add.</typeparam>
		public static T AddChildWithComponent<T>(this GameObject gameObject, GameObject prefab) where T : Component
		{
			GameObject newObject = gameObject.AddChild(prefab);

			if (newObject.GetComponent<T>() == null)
			{
				newObject.AddComponent<T>();
			}
			return newObject.GetComponent<T>();
		}

		/// <summary>
		/// Add a child object to the script with the given component, using the given prefab.
		/// </summary>
		/// <returns>The component belonging to the child that was added.</returns>
		/// <typeparam name="T">The type of component to add.</typeparam>
		public static T AddChildWithComponent<T>(this GameObject gameObject, GameObject prefab, Coordinate coordinate) where T : Component
		{
			GameObject newObject = gameObject.AddChild(prefab, coordinate);

			if (newObject.GetComponent<T>() == null)
			{
				newObject.AddComponent<T>();
			}
			return newObject.GetComponent<T>();
		}

		public static T AddChildWithComponent<T>(this GameObject gameObject, string name) where T : Component
		{
			var newObject = new GameObject(name);
			newObject.transform.SetParent(gameObject.transform, false);
			if (newObject.GetComponent<T>() == null)
			{
				newObject.AddComponent<T>();
			}
			return newObject.GetComponent<T>();
		}

		/// <summary>
		/// Calls "Destroy" on all children of this object.
		/// </summary>
		public static void DestroyAllChildren(this GameObject gameObject)
		{
			// Delete the existing children first
			var children = new List<GameObject>();
			foreach (Transform child in gameObject.transform)
			{
				children.Add(child.gameObject);
			}
			// Delete the previously existing children
			children.ForEach(x => GameObject.Destroy(x));
		}
	}
}
