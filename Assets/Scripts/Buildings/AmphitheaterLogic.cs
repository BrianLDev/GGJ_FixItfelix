using UnityEngine;

public class AmphitheaterLogic : BuildingLogicBase
{
	public override int GetSoulProduction() => LevelData[_productionLevel];
}
