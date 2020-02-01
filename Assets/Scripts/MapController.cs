using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct ConstructionSpace
{
	public Vector3Int LocalOrigin;
	public ConstructionSpaceData Data;
}

public class MapController : MonoBehaviour
{
	public Tilemap Map;

	public ConstructionSpace[] ConstructionSpaces;

	private Dictionary<Vector3Int, ConstructionSpace> _positionToConstructionSpace;

	private void Start()
	{
		_positionToConstructionSpace = new Dictionary<Vector3Int, ConstructionSpace>();

		foreach (var space in ConstructionSpaces)
		{
			Tilemap ruinShape = space.Data.RuinShape;
			foreach (Vector3Int ruinShapePosition in ruinShape.cellBounds.allPositionsWithin)
			{
				Vector3Int mapPosition = space.LocalOrigin + ruinShapePosition;
				Map.SetTile(mapPosition, ruinShape.GetTile(ruinShapePosition));
				_positionToConstructionSpace.Add(mapPosition, space);
			}
		}
	}

	public BuildingData[] GetRepairOptions(Vector3Int position)
	{
		if (!_positionToConstructionSpace.ContainsKey(position))
		{
			Debug.LogError($"Position {position} does not have a ruined building to restore");
			return null;
		}

		return _positionToConstructionSpace[position].Data.RepairOptions;
	}

	public void RepairTile(Vector3Int position, BuildingData repairOption)
	{
		if (!_positionToConstructionSpace.ContainsKey(position))
		{
			Debug.LogError($"Position {position} does not have a ruined building to restore");
			return;
		}

		ConstructionSpace space = _positionToConstructionSpace[position];
		Tilemap buildingShape = repairOption.BuildingShape;

		foreach (Vector3Int buildingPosition in buildingShape.cellBounds.allPositionsWithin)
		{
			Vector3Int mapPosition = space.LocalOrigin + buildingPosition;
			Map.SetTile(mapPosition, buildingShape.GetTile(buildingPosition));
		}

		if (repairOption.LogicPrefab != null)
		{
			GameObject buildingLogic = Instantiate(repairOption.LogicPrefab);
			buildingLogic.transform.position = Map.GetCellCenterWorld(position);

			MapControllerOnDestroyProxy proxy = buildingLogic.AddComponent<MapControllerOnDestroyProxy>();
			proxy.OnDestroyEvent.AddListener(() => ReturnToRuin(space));
		}
	}

	private void ReturnToRuin(ConstructionSpace space)
	{
		Tilemap ruinShape = space.Data.RuinShape;

		foreach (Vector3Int ruinShapePosition in ruinShape.cellBounds.allPositionsWithin)
		{
			Vector3Int mapPosition = space.LocalOrigin + ruinShapePosition;
			Map.SetTile(mapPosition, ruinShape.GetTile(ruinShapePosition));
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		foreach (ConstructionSpace space in ConstructionSpaces)
		{
			if (space.Data == null || space.Data.RuinShape == null) continue;
			BoundsInt bounds = space.Data.RuinShape.cellBounds;
			Vector3 min = Map.CellToWorld(space.LocalOrigin + bounds.min);
			Vector3 max = Map.CellToWorld(space.LocalOrigin + bounds.max);

			Gizmos.DrawCube((min + max) / 2, max - min);
		}
	}
}
