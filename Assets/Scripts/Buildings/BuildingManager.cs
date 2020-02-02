using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

[System.Serializable]
public struct ConstructionSpace
{
	public Vector3Int LocalOrigin;
	public ConstructionSpaceData Data;
}

public class BuildingManager : MonoBehaviour
{
	public Tilemap Map;

	public ConstructionSpace[] ConstructionSpaces;

	private Dictionary<Vector3Int, ConstructionSpace> _positionToConstructionSpace;
	private Dictionary<Vector3Int, GameObject> _activeBuildingLogic;

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

		_activeBuildingLogic = new Dictionary<Vector3Int, GameObject>();
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

		GameObject buildingLogic = null;
		if (repairOption.LogicPrefab != null)
		{
			buildingLogic = Instantiate(repairOption.LogicPrefab);
			buildingLogic.transform.position = Map.GetCellCenterWorld(position);

			BuildingOnDestroyProxy proxy = buildingLogic.AddComponent<BuildingOnDestroyProxy>();
			proxy.OnDestroyEvent.AddListener(() => ReturnToRuin(space));
		}

		foreach (Vector3Int buildingPosition in buildingShape.cellBounds.allPositionsWithin)
		{
			Vector3Int mapPosition = space.LocalOrigin + buildingPosition;
			Map.SetTile(mapPosition, buildingShape.GetTile(buildingPosition));
			_activeBuildingLogic.Add(mapPosition, buildingLogic);
		}
	}

	private void ReturnToRuin(ConstructionSpace space)
	{
		Tilemap ruinShape = space.Data.RuinShape;

		foreach (Vector3Int ruinShapePosition in ruinShape.cellBounds.allPositionsWithin)
		{
			Vector3Int mapPosition = space.LocalOrigin + ruinShapePosition;
			Map.SetTile(mapPosition, ruinShape.GetTile(ruinShapePosition));
			_activeBuildingLogic.Remove(mapPosition);
		}
	}

	public bool HasActiveBuilding(Vector3Int mapPosition)
	{
		return _activeBuildingLogic.ContainsKey(mapPosition);
	}

	public bool HasRuin(Vector3Int mapPosition)
	{
		return _positionToConstructionSpace.ContainsKey(mapPosition) && !HasActiveBuilding(mapPosition);
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
