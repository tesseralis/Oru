using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Util
{
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

		public static bool IsEmpty<T>(this IDictionary<T, int> ms)
		{
			return ms.Values.All(c => c == 0);
		}

		public static bool MultisetEquals<T>(this IDictionary<T, int> first, IDictionary<T, int> second)
		{
			return first.Contains(second) && second.Contains(first);
		}

		public static IDictionary<T, int> MultisetAdd<T>(this IDictionary<T, int> ms, T item, int amount)
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
		public static IDictionary<T, int> MultisetAdd<T>(this IDictionary<T, int> first, IDictionary<T, int> second)
		{
			IDictionary<T, int> result = new Dictionary<T, int>(first);
			foreach (var entry in second)
			{
				result = result.MultisetAdd(entry.Key, entry.Value);
			}
			return result;
		}

		public static IDictionary<T, int> MultisetSubtract<T>(this IDictionary<T, int> ms, T item, int amount)
		{
			var result = new Dictionary<T, int>(ms);
			if (ms.ContainsKey(item))
			{
				var newCount = ms[item] - amount;
				if (newCount > 0)
				{
					result[item] = newCount;
				}
				else
				{
					result.Remove(item);
				}
			}
			return result;
		}

		/// <summary>
		/// Return the set difference of the first multiset from the second.
		/// </summary>
		public static IDictionary<T, int> MultisetSubtract<T>(this IDictionary<T, int> first, IDictionary<T, int> second)
		{
			IDictionary<T, int> result = new Dictionary<T, int>(first);
			foreach (var entry in second)
			{
				result = result.MultisetSubtract(entry.Key, entry.Value);
			}
			return result;
		}

		public static bool Contains<T>(this IDictionary<T, int> set, T item)
		{
			return set.ContainsKey(item) && set[item] > 0;
		}

		// Check if one multiset contains another
		public static bool Contains<T>(this IDictionary<T, int> superset, IDictionary<T, int> subset)
		{
			return subset.All(entry => entry.Value == 0 || (superset.ContainsKey(entry.Key) && superset[entry.Key] >= entry.Value));
		}
	}
}
