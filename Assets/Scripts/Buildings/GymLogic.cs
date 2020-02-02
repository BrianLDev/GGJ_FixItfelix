using UnityEngine;

public class GymLogic : BuildingLogicBase
{
	public override int GetHealthBonusPercent() => LevelData[_productionLevel];
}
