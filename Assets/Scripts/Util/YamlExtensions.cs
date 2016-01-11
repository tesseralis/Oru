using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using YamlDotNet.Serialization;
using YamlDotNet.RepresentationModel;

// FIXME resolve all the weird type errors (I have to specify types to get Unity to build, but not Mono)
namespace Util
{
	public static class YamlExtensions
	{
		public static YamlNode GetChild(this YamlMappingNode map, string key)
		{
			return map.Children[new YamlScalarNode(key)];
		}

		public static YamlMappingNode GetMapping(this YamlMappingNode map, string key)
		{
			return (YamlMappingNode)map.GetChild(key);
		}

		public static YamlSequenceNode GetSequence(this YamlMappingNode map, string key)
		{
			return (YamlSequenceNode)map.GetChild(key);
		}

		public static string GetString(this YamlMappingNode map, string key)
		{
			return map.GetChild(key).ToString();
		}

		public static int ToInt(this YamlNode node)
		{
			return Int32.Parse(node.ToString());
		}

		public static T ToEnum<T>(this YamlNode node)
		{
			return (T)Enum.Parse(typeof(T), node.ToString());
		}

		public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this YamlMappingNode map, Func<YamlNode, TKey> tfunc, Func<YamlNode, TValue> vfunc)
		{
			return map.ToDictionary(x => tfunc(x.Key), x => vfunc(x.Value));
		}

		public static IDictionary<Coordinate, T> ToCoordinateMap<T>(this YamlMappingNode map, Func<YamlNode, T> func)
		{
			return map.ToDictionary<Coordinate, T>(DeserializeCoordinate, func);
		}

		private static Coordinate DeserializeCoordinate(YamlNode node)
		{
			int[] coords = node.ToString().Split(',').Select<string, int>(Int32.Parse).ToArray();
			return new Coordinate(coords[0], coords[1]);
		}
	}
}