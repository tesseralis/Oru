public struct Coordinate
{
	public Coordinate(int _x, int _z)
	{
		x = _x;
		z = _z;
	}
	public int x { get; set; }
	public int z { get; set; }
	
	public override bool Equals( object obj )
	{
		return obj is Coordinate && this == (Coordinate)obj;
   
	}

	public static bool operator ==(Coordinate c1, Coordinate c2) 
	{
		return c1.x == c2.x && c1.z == c2.z;
	}
	public static bool operator !=(Coordinate c1, Coordinate c2) 
	{
		return !(c1 == c2);
	}

	public static Coordinate operator +(Coordinate c1, Coordinate c2)
	{
		return new Coordinate(c1.x + c2.x, c1.z + c2.z);
	}
	
	public override int GetHashCode()
	{
		int hash = 13;
		hash = (hash * 7) + x;
		hash = (hash * 11) + z;
		return hash;
	}

	public override string ToString ()
	{
		return string.Format ("[Coordinate: x={0}, z={1}]", x, z);
	}
}
