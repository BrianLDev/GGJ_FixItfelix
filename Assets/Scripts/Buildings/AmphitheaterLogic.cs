using UnityEngine;

public class AmphitheaterLogic : BuildingLogicBase
{
	public override int GetSoulProduction() => LevelData[ProductionLevel];

    public override string GetBuildingType() => "amp";
}
