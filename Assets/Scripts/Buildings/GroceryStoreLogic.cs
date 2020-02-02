using UnityEngine;

public class GroceryStoreLogic : BuildingLogicBase
{
	public override int GetProductionBonusPercent() => LevelData[_productionLevel];
}
