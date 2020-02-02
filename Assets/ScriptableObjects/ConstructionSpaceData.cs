using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

// A building space represents a certain size of ruins that you can replace with a building
// e.g. A 2x2 ruin that can be replaced by either a house or a library
[CreateAssetMenu(menuName = "Scriptable Objects/Building Space")]
public class ConstructionSpaceData : ScriptableObject
{
	public Tilemap RuinShape;
	public BuildingData[] RepairOptions;
}

[CustomEditor(typeof(ConstructionSpaceData))]
public class ConstructionSpaceEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUIStyle errorStyle = new GUIStyle(EditorStyles.label);
		errorStyle.normal.textColor = Color.red;

		ConstructionSpaceData buildingSpace = target as ConstructionSpaceData;
		if (buildingSpace.RuinShape == null)
		{
			EditorGUILayout.LabelField("Building space does not have ruin shape", errorStyle);
			return;
		}

		BoundsInt ruinBounds = buildingSpace.RuinShape.cellBounds;

		for (int i = 0; i < buildingSpace.RepairOptions.Length; i++)
		{
			BuildingData buildingData = buildingSpace.RepairOptions[i];

			if (buildingData == null) continue;

			string errorMessage = $"Option {buildingData} (index {i}) error";

			if (buildingData.BuildingShape == null)
			{
				EditorGUILayout.LabelField(errorMessage, errorStyle);
				EditorGUILayout.LabelField("It does not have an associated shape.", errorStyle);
				continue;
			}

			BoundsInt buildingBounds = buildingData.BuildingShape.cellBounds;

			if (buildingBounds.size != ruinBounds.size)
			{
				EditorGUILayout.LabelField(errorMessage, errorStyle);
				EditorGUILayout.LabelField($"It does not have the correct size.", errorStyle);
			}
		}
	}
}
