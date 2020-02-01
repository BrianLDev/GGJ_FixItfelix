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
	public int BaseT1MaxHealth;
	public int BaseT2MaxHealth;
	public int BaseT3MaxHealth;
}
