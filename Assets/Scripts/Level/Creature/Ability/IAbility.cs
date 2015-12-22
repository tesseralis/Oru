using System;

/// <summary>
/// An ability represents a special action that a specific creature can do.
/// </summary>
public interface IAbility
{
	/// <summary>
	/// Use the ability on the specified coordinate.
	/// </summary>
	/// <param name="coordinate">Coordinate.</param>
	void Use(Coordinate coordinate);
}