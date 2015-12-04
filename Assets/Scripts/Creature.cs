using UnityEngine;
using System.Collections;

public class Creature : MonoBehaviour
{

	public TerrainType[] allowedTerrain = {TerrainType.Grass};

	void OnMouseDown()
	{
		GameManager.gm.SetCurrentCreature (gameObject);
	}

}
