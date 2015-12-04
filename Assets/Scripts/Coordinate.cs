public class Coordinate
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
		var other = obj as Coordinate;
		if( other == null ) return false;
		
		return Equals (other);             
	}
	
	public override int GetHashCode()
	{
		int hash = 13;
		hash = (hash * 7) + x;
		hash = (hash * 11) + z;
		return hash;
	}
	
	public bool Equals( Coordinate other )
	{
		if    ( other == null )                    return false;
		if    ( GetType() != other.GetType()    )    return false;
		if    ( ReferenceEquals (this, other)    )    return true;
		if    ( x == other.x && z == other.z    )    return true;
		return false;
	}
}