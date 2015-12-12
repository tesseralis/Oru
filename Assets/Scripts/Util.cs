using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Util
{
	// TODO Move this into its own "multiset" class
	// Defines extension methods that allow dictionaries to be treated as multisets
	public static class Multiset
	{
		/// <summary>
		/// Return an empty multiset.
		/// </summary>
		/// <typeparam name="T">The type this multiset stores.</typeparam>
		public static IDictionary<T, int> Empty<T>()
		{
			return new Dictionary<T, int>();
		}

		// TODO figure out performance implications of making so many dictionaries
		public static IDictionary<T, int> AddMultiple<T>(this IDictionary<T, int> ms, T item, int amount)
		{
			var result = new Dictionary<T, int>(ms);
			if (ms.ContainsKey(item))
			{
				result[item] += amount;
			}
			else
			{
				result[item] = amount;
			}
			return result;
		}

		// Adds up the two multisets into a third multiset.
		public static IDictionary<T, int> AddMultiple<T>(this IDictionary<T, int> first, IDictionary<T, int> second)
		{
			IDictionary<T, int> result = new Dictionary<T, int>(first);
			foreach (var entry in second)
			{
				result = result.AddMultiple(entry.Key, entry.Value);
			}
			return result;
		}

		// Check if one multiset contains another
		public static bool Contains<T>(this IDictionary<T, int> superset, IDictionary<T, int> subset)
		{
			return subset.All(entry => superset.ContainsKey(entry.Key) && superset[entry.Key] >= entry.Value);
		}
	}

	// Defines extension methods related to coordinates
	public static class Coordinates
	{
		public static Coordinate Coordinate(this MonoBehaviour script)
		{
			return GameManager.gm.ToGridCoordinate(script.gameObject.transform.position);
		}
	}
}
