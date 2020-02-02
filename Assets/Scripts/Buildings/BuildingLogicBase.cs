using UnityEngine;

public abstract class BuildingLogicBase : MonoBehaviour
{
	[HideInInspector]
	public BuildingManager BuildingManager;

	public int ProductionLevel { get; protected set; }
	public int[] LevelData = new int[3];

	protected virtual void Start()
	{
		ProductionLevel = 0;
	}

	public virtual int GetMindProduction() => 0;

	public virtual int GetSoulProduction() => 0;

	public virtual int GetHealthBonusPercent() => 0;

	public virtual int GetProductionBonusPercent() => 0;

	public virtual bool CanUpgradeProduction() => ProductionLevel + 1 < LevelData.Length;

	public virtual void DoUpgradeProduction()
	{
		ProductionLevel += 1;
	}

    public virtual string GetBuildingType() => "";
}
