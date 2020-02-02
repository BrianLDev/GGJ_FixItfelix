using UnityEngine;

public class LibraryLogic : BuildingLogicBase
{
	public override int GetMindProduction() => LevelData[_productionLevel];

    public override string GetBuildingType() => "library";
}
