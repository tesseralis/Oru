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

	/// <summary>
	/// Activate the passive version of this ability.
	/// </summary>
	void Passive();

	/// <summary>
	/// Description of what happens when you use the ability.
	/// </summary>
	/// <returns>The ability description.</returns>
	string Description();
}
