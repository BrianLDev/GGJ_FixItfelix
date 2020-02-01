using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct BuildingSlot
{
	public Vector2Int Center;
	public Vector2Int[] Positions;
}

public class MapController : MonoBehaviour
{
	public Tilemap Map;
	public TileBase RuinTile;
	public BuildingSlot[] BuildingSlots;

	private void Start()
	{
		foreach (BuildingSlot slot in BuildingSlots)
		{
			foreach (Vector2Int offset in slot.Positions)
			{
				Vector2Int position = slot.Center + offset;
				Map.SetTile(new Vector3Int(position.x, position.y, 0), RuinTile);
			}
		}
	}

	public void RepairTile(Vector2Int mapPosition)
	{
	}
}
