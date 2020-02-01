using UnityEngine;
using UnityEditor;

// A building space represents a certain size of ruins that you can replace with a building
// e.g. A 2x2 ruin that can be replaced by either a house or a library
[CreateAssetMenu(menuName="Scriptable Objects/Building Space")]
public class BuildingSpace : ScriptableObject
{
	/// <summary>
	/// Should contain a tilemap
	/// </summary>
	public GameObject RuinShape;
	public BuildingData[] RepairOptions;
}
