using UnityEngine;

public class AmphitheaterLogic : BuildingLogicBase
{
	public override int GetSoulProduction() => LevelData[_productionLevel];

    public override string GetBuildingType() => "amp";
}
