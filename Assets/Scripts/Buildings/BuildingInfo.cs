using UnityEngine;

public enum BuildingType
{
	Mind,
	Body,
	Soul,
	Vice
}

public class BuildingInfo : MonoBehaviour
{
	public BuildingType BuildingType;
	public int BaseCost;
}
