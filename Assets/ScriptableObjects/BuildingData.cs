﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

// A BuildingData object represents a certain kind of building
// e.g. A library, or a house
[CreateAssetMenu(menuName="Scriptable Objects/Building Data")]
public class BuildingData : ScriptableObject
{
	public Tilemap BuildingShape;

	public GameObject LogicPrefab;

	private void OnValidate()
	{
		BuildingShape.CompressBounds();
	}
}

[CustomEditor(typeof(BuildingData))]
public class BuildingDataEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		BuildingData building = target as BuildingData;
		if (building.BuildingShape == null)
		{
			GUIStyle errorStyle = new GUIStyle(EditorStyles.label);
			errorStyle.normal.textColor = Color.red;
			EditorGUILayout.LabelField("Building is missing a shape reference", errorStyle);
		}
	}
}