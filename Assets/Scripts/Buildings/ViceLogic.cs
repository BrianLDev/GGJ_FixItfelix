using UnityEngine;

public class ViceLogic : BuildingLogicBase
{
	public override bool CanUpgradeProduction() => false;
	public override int GetMindProduction() => -100;
	public override int GetSoulProduction() => -10;
}
