using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class TestRepairBuilding : MonoBehaviour, IPointerClickHandler
{
	public BuildingManager BuildingManager;

	public void OnPointerClick(PointerEventData data)
	{
		Tilemap map = GetComponent<Tilemap>();

		Vector3Int mapPosition = map.WorldToCell(data.pointerCurrentRaycast.worldPosition);

		BuildingManager.RepairTile(mapPosition, BuildingManager.GetRepairOptions(mapPosition)[0]);
	}
}
