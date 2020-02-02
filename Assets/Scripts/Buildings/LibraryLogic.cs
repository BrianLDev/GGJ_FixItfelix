using UnityEngine;

public class LibraryLogic : BuildingLogicBase
{
	public override int GetMindProduction() => LevelData[ProductionLevel];

    public override string GetBuildingType() => "library";
}
