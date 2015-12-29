using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Util
{

	// Defines extension methods related to coordinates
	public static class GameObjectUtils
	{
		public static Coordinate Coordinate(this GameObject gameObject)
		{
			var cellSize = LevelManager.level.cellSize;
			var position = gameObject.gameObject.transform.position;
			return new Coordinate(Mathf.RoundToInt(position.x / cellSize), Mathf.RoundToInt(position.z / cellSize));
		}

		public static void SetPosition(this GameObject gameObject, Coordinate coordinate)
		{
			var cellSize = LevelManager.level.cellSize;
			gameObject.transform.position = new Vector3(coordinate.x * cellSize, gameObject.transform.position.y, coordinate.z * cellSize);
		}

		/// <summary>
		/// Add a child object to the script, using the given prefab.
		/// </summary>
		/// <returns>The child GameObject.</returns>
		public static GameObject AddChild(this GameObject gameObject, GameObject prefab, Coordinate coordinate)
		{
			var cellSize = LevelManager.level.cellSize;
			var position = new Vector3(coordinate.x * cellSize, 0, coordinate.z * cellSize);
			GameObject newObject = (GameObject)GameObject.Instantiate(prefab, position, Quaternion.identity);
			newObject.transform.SetParent(gameObject.transform);
			return newObject;
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
