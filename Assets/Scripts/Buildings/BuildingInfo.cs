using UnityEngine;

public enum BuildingType
{
	Mind,
	Body,
	Soul
}

public class BuildingInfo : MonoBehaviour
{
	public BuildingType BuildingType;
	public int BaseCost;
}
