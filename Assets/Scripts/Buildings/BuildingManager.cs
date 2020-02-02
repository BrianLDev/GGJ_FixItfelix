using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

[Serializable]
public struct ConstructionSpace
{
	public Vector3Int LocalOrigin;
	public ConstructionSpaceData Data;
}

[Serializable]
public struct RepairConfig
{
	public int HealthNumerator;
	public int HealthDenominator;
	public int CostNumerator;
	public int CostDenominator;
}

public class BuildingManager : MonoBehaviour
{
	public Tilemap Map;

	public ConstructionSpace[] ConstructionSpaces;

	public RepairConfig[] RepairCosts;

	private Dictionary<Vector3Int, ConstructionSpace> _positionToConstructionSpace;
	private Dictionary<Vector3Int, GameObject> _positionToBuildingLogic;
	private HashSet<GameObject> _activeBuildingLogic;
	private PlayerStatsScript playerStats;
    private AudioManagerScript audioManager;

	public int CachedHealthBonusPercent { get; private set; }

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

		_positionToBuildingLogic = new Dictionary<Vector3Int, GameObject>();
		_activeBuildingLogic = new HashSet<GameObject>();

		playerStats = this.transform.parent.gameObject.GetComponentInChildren<PlayerStatsScript>();
        audioManager = this.transform.parent.gameObject.GetComponentInChildren<AudioManagerScript>();
    }

	public BuildingData[] GetConstructionOptions(Vector3Int position)
	{
		if (!_positionToConstructionSpace.ContainsKey(position))
		{
			Debug.LogError($"Position {position} does not have a ruined building to restore");
			return null;
		}

		return _positionToConstructionSpace[position].Data.RepairOptions;
	}

	public void ConstructBuildingOnTile(Vector3Int position, BuildingData buildingOption)
	{
		if (!HasRuin(position))
		{
			Debug.LogError($"Position {position} does not have a ruined building to restore");
			return;
		}

		ConstructionSpace space = _positionToConstructionSpace[position];
		Tilemap buildingShape = buildingOption.BuildingShape;

		GameObject buildingLogic = null;
		if (buildingOption.LogicPrefab != null)
		{
			int buildingCost = buildingOption.LogicPrefab.GetComponent<BuildingInfo>().BaseCost;
			if (playerStats.GetMind() - buildingCost < 0)
			{
				return;
			}
			playerStats.UpdateMind(buildingCost * -1.0f);

			buildingLogic = Instantiate(buildingOption.LogicPrefab);
			buildingLogic.transform.position = Map.GetCellCenterWorld(position);
			_activeBuildingLogic.Add(buildingLogic);
			OnHealthBonusMayHaveChanged();

			buildingLogic.GetComponent<BuildingHealth>().BuildingManager = this;
			buildingLogic.GetComponent<BuildingLogicBase>().BuildingManager = this;

            audioManager.PlayBuildingBuilt();

			BuildingOnDestroyProxy proxy = buildingLogic.AddComponent<BuildingOnDestroyProxy>();
			proxy.OnDestroyEvent.AddListener(() => ReturnToRuin(space, buildingLogic));
		}

		BoundsInt ruinBounds = _positionToConstructionSpace[position].Data.RuinShape.cellBounds;

		foreach (Vector3Int buildingPosition in buildingShape.cellBounds.allPositionsWithin)
		{
			Vector3Int buildingOffsetCompensation = ruinBounds.min - buildingShape.cellBounds.min;
			Vector3Int mapPosition = space.LocalOrigin + buildingOffsetCompensation + buildingPosition;
			Map.SetTile(mapPosition, buildingShape.GetTile(buildingPosition));
			_positionToBuildingLogic.Add(mapPosition, buildingLogic);
		}
	}

	private void ReturnToRuin(ConstructionSpace space, GameObject buildingLogic)
	{
		Tilemap ruinShape = space.Data.RuinShape;

		if (Map != null)
		{
			foreach (Vector3Int ruinShapePosition in ruinShape.cellBounds.allPositionsWithin)
			{
				Vector3Int mapPosition = space.LocalOrigin + ruinShapePosition;
				Map.SetTile(mapPosition, ruinShape.GetTile(ruinShapePosition));
				_positionToBuildingLogic.Remove(mapPosition);
			}
            audioManager.PlayBuildingDestroyed();
		}

		if (buildingLogic != null)
		{
			_activeBuildingLogic.Remove(buildingLogic);
			OnHealthBonusMayHaveChanged();
		}
	}

	public enum BuildingAction
	{
		UPGRADE_HEALTH,
		UPGRADE_PRODUCTION,
		REPAIR
	}

	public BuildingAction[] GetBuildingActionOptions(Vector3Int position)
	{
		if (!HasActiveBuilding(position))
		{
			Debug.LogError($"Position {position} does not have a working building to act on");
			return null;
		}

		List<BuildingAction> options = new List<BuildingAction>();
		GameObject buildingLogic = _positionToBuildingLogic[position];
		if (buildingLogic.GetComponent<BuildingLogicBase>().CanUpgradeProduction())
		{
			options.Add(BuildingAction.UPGRADE_PRODUCTION);
		}
		BuildingHealth buildingHealth = buildingLogic.GetComponent<BuildingHealth>();
		if (buildingHealth.CanUpgradeHealth())
		{
			options.Add(BuildingAction.UPGRADE_HEALTH);
		}
		if (buildingHealth.CurrentHealth < buildingHealth.MaxHealth)
		{
			options.Add(BuildingAction.REPAIR);
		}
		return options.ToArray();
	}

	private int GetRepairCost(BuildingHealth health)
	{
		int healthLost = health.MaxHealth - health.CurrentHealth;
		int buildingCost = health.GetComponent<BuildingInfo>().BaseCost;
		foreach (RepairConfig config in RepairCosts)
		{
			if (healthLost * config.HealthDenominator < health.MaxHealth * config.HealthNumerator)
			{
				return buildingCost * config.CostNumerator / config.CostDenominator;
			}
		}
		Debug.LogError("Unexpected branch in GetRepairCost");
		Debug.LogError($"Building health {health.CurrentHealth} max health {health.MaxHealth} health lost {healthLost}");
		Debug.LogError("Defaulting to full cost");
		return buildingCost;
	}

	public void ExecuteActionOnBuilding(Vector3Int position, BuildingAction action)
	{
		GameObject buildingLogic = _positionToBuildingLogic[position];
		BuildingHealth health = buildingLogic.GetComponent<BuildingHealth>();
		BuildingLogicBase logic = buildingLogic.GetComponent<BuildingLogicBase>();

		switch (action)
		{
		case BuildingAction.UPGRADE_HEALTH:
			health.DoUpgradeHealth();
			return;
		case BuildingAction.UPGRADE_PRODUCTION:
			logic.DoUpgradeProduction();
			return;
		case BuildingAction.REPAIR:
			int cost = GetRepairCost(health);
			if (cost > playerStats.GetMind())
			{
				return;
			}
			health.DoRepair();
			playerStats.UpdateMind(-cost);
			return;
		}
	}

	private int AggregateBuildingStats(Func<BuildingLogicBase, int> statGetter)
	{
		return _activeBuildingLogic.Aggregate(0, (sum, buildingLogic) =>
	{
		if (buildingLogic == null) return sum;
		BuildingLogicBase logic = buildingLogic.GetComponent<BuildingLogicBase>();
		if (logic == null) return sum;
		return sum + statGetter(logic);
	});
	}

	public int GetTotalBaseMindProduction() => AggregateBuildingStats(logic => logic.GetMindProduction());

	public int GetTotalBaseSoulProduction() => AggregateBuildingStats(logic => logic.GetSoulProduction());

	public int ComputeTotalHealthBonusPercent() => AggregateBuildingStats(logic => logic.GetHealthBonusPercent());

	public int GetTotalProductionBonusPercent() => AggregateBuildingStats(logic => logic.GetProductionBonusPercent());

	public int GetMindProductionWithBonus()
		=> GetTotalBaseMindProduction() * (100 + GetTotalProductionBonusPercent()) / 100;

	public int GetSoulProductionWithBonus()
		=> GetTotalBaseSoulProduction() * (100 + GetTotalProductionBonusPercent()) / 100;

	// To be called every time the health bonus may have changed, triggers a recalculation of health bonus and applies it to buildings if necessary
	public void OnHealthBonusMayHaveChanged()
	{
		int oldHealthBonusPercent = CachedHealthBonusPercent;
		CachedHealthBonusPercent = ComputeTotalHealthBonusPercent();

		if (CachedHealthBonusPercent != oldHealthBonusPercent)
		{
			foreach (GameObject buildingLogic in _activeBuildingLogic)
			{
				buildingLogic.GetComponent<BuildingHealth>().OnHealthBonusChanged(oldHealthBonusPercent, CachedHealthBonusPercent);
			}
		}
	}

	public GameObject[] GetBuildings()
	{
		return _activeBuildingLogic.ToArray();
	}

	public bool HasActiveBuilding(Vector3Int mapPosition)
	{
		return _positionToBuildingLogic.ContainsKey(mapPosition);
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
