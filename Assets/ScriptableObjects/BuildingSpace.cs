using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

// A building space represents a certain size of ruins that you can replace with a building
// e.g. A 2x2 ruin that can be replaced by either a house or a library
[CreateAssetMenu(menuName = "Scriptable Objects/Building Space")]
public class BuildingSpace : ScriptableObject
{
	public Tilemap RuinShape;
	public BuildingData[] RepairOptions;

	private void OnValidate()
	{
		RuinShape.CompressBounds();
	}
}

[CustomEditor(typeof(BuildingSpace))]
public class BuildingSpaceEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUIStyle errorStyle = new GUIStyle(EditorStyles.label);
		errorStyle.normal.textColor = Color.red;

		BuildingSpace buildingSpace = target as BuildingSpace;
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

			string errorMessage = $"Option {buildingData} (index {i}) has issue:";

			if (buildingData.BuildingShape == null)
			{
				EditorGUILayout.LabelField(errorMessage, errorStyle);
				EditorGUILayout.LabelField("It does not have an associated shape.", errorStyle);
				continue;
			}

			BoundsInt buildingBounds = buildingData.BuildingShape.cellBounds;

			if (buildingBounds.size != ruinBounds.size && buildingBounds.min != ruinBounds.min)
			{
				EditorGUILayout.LabelField(errorMessage, errorStyle);
				EditorGUILayout.LabelField($"It does not have the correct size or position.", errorStyle);
			}
			else if (buildingBounds.size != ruinBounds.size)
			{
				EditorGUILayout.LabelField(errorMessage, errorStyle);
				EditorGUILayout.LabelField($"It does not have the correct size.", errorStyle);
			}
			else if (buildingBounds.min != ruinBounds.min)
			{
				EditorGUILayout.LabelField(errorMessage, errorStyle);
				EditorGUILayout.LabelField($"It does not have the correct position.", errorStyle);
				EditorGUILayout.LabelField($"It is offset by {buildingBounds.min - ruinBounds.min}.", errorStyle);
			}
		}
	}
}
