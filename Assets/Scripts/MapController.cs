using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct BuildingTile
{
	public Vector2Int Offset;
	public TileBase RepairedTile;
}

[System.Serializable]
public struct BuildingSlot
{
	public Vector2Int Center;
	public BuildingTile[] Tiles;
}

public class MapController : MonoBehaviour
{
	public Tilemap Map;
	public TileBase RuinTile;
	public BuildingSlot[] BuildingSlots;

	private Dictionary<Vector2Int, BuildingSlot> _ruinRestoreData;

	private void Start()
	{
		_ruinRestoreData = new Dictionary<Vector2Int, BuildingSlot>();

		foreach (BuildingSlot slot in BuildingSlots)
		{
			foreach (BuildingTile tile in slot.Tiles)
			{
				Vector2Int position = slot.Center + tile.Offset;
				Map.SetTile(new Vector3Int(position.x, position.y, 0), RuinTile);
				_ruinRestoreData.Add(position, slot);
			}
		}
	}

	public void RepairTile(Vector3Int position)
	{
		Vector2Int mapPosition = new Vector2Int(position.x, position.y);

		if (!_ruinRestoreData.ContainsKey(mapPosition))
		{
			Debug.LogError($"Position {mapPosition} does not have a ruined building to restore");
			return;
		}

		BuildingSlot slot = _ruinRestoreData[mapPosition];

		foreach (BuildingTile tile in slot.Tiles)
		{
			Vector2Int tilePosition = slot.Center + tile.Offset;
			Map.SetTile(new Vector3Int(tilePosition.x, tilePosition.y, 0), tile.RepairedTile);
		}
	}
}
