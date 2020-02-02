using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public struct TileOption
{
	public TileBase Tile;
	public float Weight;
}

public class MapRandomizer : MonoBehaviour
{
	public Tilemap Map;
	public TileOption[] OptionsPool;

	public void RandomizeTiles()
	{
		BuildingManager buildingManager = GetComponent<BuildingManager>();

		foreach (Vector3Int position in Map.cellBounds.allPositionsWithin)
		{
			if (Map.HasTile(position) && !buildingManager.HasRuin(position) && !buildingManager.HasActiveBuilding(position))
			{
				Map.SetTile(position, ChooseTile());
			}
		}
	}

	private TileBase ChooseTile()
	{
		float totalWeight = OptionsPool.Sum(option => option.Weight);
		float chosenWeight = totalWeight * UnityEngine.Random.value;
		foreach (TileOption option in OptionsPool)
		{
			chosenWeight -= option.Weight;
			if (chosenWeight < 0)
			{
				return option.Tile;
			}
		}
		Debug.LogWarning("Unexpected branch in ChooseTile");
		Debug.LogWarning($"Total weight {totalWeight}");
		return OptionsPool[OptionsPool.Length - 1].Tile;
	}
}
