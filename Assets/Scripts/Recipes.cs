using System.Collections.Generic;

/// <summary>
/// Defines the static recipes needed to create each creature.
/// </summary>
public static class Recipes
{
	public static IDictionary<ResourceType, int> dragonRecipe = new Dictionary<ResourceType, int>()
	{
		{ResourceType.Red, 5}
	};

	public static IDictionary<ResourceType, int> duckRecipe = new Dictionary<ResourceType, int>()
	{
		{ResourceType.Yellow, 1}
	};
}