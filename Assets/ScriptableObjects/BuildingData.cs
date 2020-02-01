using UnityEngine;
using UnityEngine.Tilemaps;

// A BuildingData object represents a certain kind of building
// e.g. A library, or a house
[CreateAssetMenu(menuName="Scriptable Objects/Building Data")]
public class BuildingData : ScriptableObject
{
	public Tilemap BuildingShape;
}
