using UnityEngine;

public class GymLogic : BuildingLogicBase
{
	public override int GetHealthBonusPercent() => LevelData[_productionLevel];

	public override void DoUpgradeProduction()
	{
		base.DoUpgradeProduction();
		BuildingManager.OnHealthBonusMayHaveChanged();
	}

    public override string GetBuildingType() => "gym";
}
