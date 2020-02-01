using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class TestRepairBuilding : MonoBehaviour, IPointerClickHandler
{
	public MapController MapController;

	public void OnPointerClick(PointerEventData data)
	{
		Tilemap map = GetComponent<Tilemap>();

		Vector3Int mapPosition = map.WorldToCell(data.pointerCurrentRaycast.worldPosition);

		MapController.RepairTile(mapPosition, MapController.GetRepairOptions(mapPosition)[0]);
	}
}
