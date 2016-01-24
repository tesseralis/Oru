using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public struct CreatureType
{
	public string name;

	public CreatureType(string _name)
	{
		name = _name;
	}

	public override int GetHashCode()
	{
		return name.GetHashCode();
	}

	public override bool Equals( object obj )
	{
		return obj is CreatureType && this == (CreatureType)obj;
	}

	public static bool operator ==(CreatureType c1, CreatureType c2) 
	{
		return c1.name == c2.name;
	}
	public static bool operator !=(CreatureType c1, CreatureType c2) 
	{
		return !(c1 == c2);
	}

	public override string ToString ()
	{
		return name;
	}
}
