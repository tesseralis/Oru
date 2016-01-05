using System;

public interface IAbilityDefinition
{
	// Get a user-friendly description of this ability
	string Description();

	IAbility AddToCreature(Creature creature);
}

