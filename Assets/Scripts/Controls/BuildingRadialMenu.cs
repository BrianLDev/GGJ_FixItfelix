using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingRadialMenu : MonoBehaviour, IInitializePotentialDragHandler, IDragHandler, IEndDragHandler
{
	public BuildingManager BuildingManager;
	public Canvas Canvas;
	public GameObject SelectionCoinPrefab;

	private bool _waitingUserSelection = false;

	public float CoinsExpandTime;
	public float CoinNormalRange;
	public float CoinHighlightScale;

	public void OnInitializePotentialDrag(PointerEventData data)
	{
		Vector3Int mapPosition = BuildingManager.Map.WorldToCell(data.pointerCurrentRaycast.worldPosition);

		if (BuildingManager.HasRuin(mapPosition))
		{
			StartCoroutine(SelectBuildingMenu(mapPosition));
		}
		else if (BuildingManager.HasActiveBuilding(mapPosition))
		{

		}
	}
	private Vector2 WorldToCanvasPosition(Vector2 worldPosition)
	{
		return Camera.main.WorldToScreenPoint(worldPosition)
			   / new Vector2(Screen.width, Screen.height)
			   * Canvas.pixelRect.size;
	}

	private IEnumerator SelectBuildingMenu(Vector3Int mapPosition)
	{
		Vector3 worldPosition = BuildingManager.Map.GetCellCenterWorld(mapPosition);
		Vector3 canvasPosition = WorldToCanvasPosition(worldPosition);

		BuildingData[] options = BuildingManager.GetRepairOptions(mapPosition);

		Coroutine[] extendCoin = new Coroutine[options.Length];
		for (int i = 0; i < options.Length; i++)
		{
			extendCoin[i] = StartCoroutine(ExtendCoin(
				canvasPosition,
				options[i].PreviewImage,
				 Mathf.Lerp(0, 2 * Mathf.PI,
				  (float)i / options.Length
				  )));
		}
		foreach (Coroutine coroutine in extendCoin)
		{
			yield return coroutine;
		}
	}

	private IEnumerator ExtendCoin(Vector3 startPosition, Sprite sprite, float angle)
	{
		float startTime = Time.time;

		GameObject coin = Instantiate(SelectionCoinPrefab, Canvas.transform);
		coin.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = sprite;

		RectTransform coinTransform = coin.GetComponent<RectTransform>();
		coinTransform.anchoredPosition = startPosition;
		Vector3 endPosition = startPosition + new Vector3(-Mathf.Sin(angle), Mathf.Cos(angle)) * CoinNormalRange;

		while (Time.time - startTime < CoinsExpandTime)
		{
			float progress = (Time.time - startTime) / CoinsExpandTime;

			coinTransform.localScale = Vector3.one * progress;
			coinTransform.anchoredPosition = Vector3.Lerp(startPosition, endPosition, progress);

			yield return null;
		}

		coinTransform.localScale = Vector3.one;
		coinTransform.anchoredPosition = endPosition;
	}

	public void OnDrag(PointerEventData data)
	{

	}

	public void OnEndDrag(PointerEventData data)
	{
		Debug.Log("Ending drag");
	}
}
