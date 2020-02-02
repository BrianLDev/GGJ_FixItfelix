using UnityEngine;

public class GroceryStoreLogic : BuildingLogicBase
{
	public override int GetProductionBonusPercent() => LevelData[ProductionLevel];

    public override string GetBuildingType() => "market";
}
