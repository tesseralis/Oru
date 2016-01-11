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

namespace Util
{
	public static class YamlExtensions
	{
		public static YamlScalarNode AsScalar(this YamlNode node)
		{
			return (YamlScalarNode)node;
		}

		public static YamlMappingNode AsMapping(this YamlNode node)
		{
			return (YamlMappingNode)node;
		}

		public static YamlSequenceNode AsSequence(this YamlNode node)
		{
			return (YamlSequenceNode)node;
		}

		public static YamlNode GetChild(this YamlMappingNode map, string key)
		{
			return map.Children[new YamlScalarNode(key)];
		}

		public static YamlMappingNode GetMapping(this YamlMappingNode map, string key)
		{
			return map.GetChild(key).AsMapping();
		}

		public static YamlSequenceNode GetSequence(this YamlMappingNode map, string key)
		{
			return map.GetChild(key).AsSequence();
		}

		public static string GetString(this YamlMappingNode map, string key)
		{
			return map.GetChild(key).AsScalar().Value;
		}

		public static int ToInt(this YamlNode node)
		{
			return Int32.Parse(node.AsScalar().Value);
		}

		public static T ToEnum<T>(this YamlNode node)
		{
			return (T)Enum.Parse(typeof(T), node.AsScalar().Value);
		}

		public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this YamlNode node,
			Func<YamlNode, TKey> tfunc, Func<YamlNode, TValue> vfunc)
		{
			return node.AsMapping().ToDictionary(x => tfunc(x.Key), x => vfunc(x.Value));
		}
	}
}