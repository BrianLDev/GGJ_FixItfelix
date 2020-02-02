using UnityEngine;

public abstract class BuildingLogicBase : MonoBehaviour
{
	[HideInInspector]
	public BuildingManager BuildingManager;

	protected int _productionLevel;
	public int[] LevelData = new int[3];

	protected virtual void Start()
	{
		_productionLevel = 0;
	}

	public virtual int GetMindProduction() => 0;

	public virtual int GetSoulProduction() => 0;

	public virtual int GetHealthBonusPercent() => 0;

	public virtual int GetProductionBonusPercent() => 0;

	public virtual bool CanUpgradeProduction() => _productionLevel + 1 < LevelData.Length;

	public virtual void DoUpgradeProduction()
	{
		_productionLevel += 1;
	}
}
