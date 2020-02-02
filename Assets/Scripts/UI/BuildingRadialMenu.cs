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
	public int? SelectedIndex;

	public float CoinsExpandTime;
	public float CoinNormalRange;
	public float CoinHighlightScale;
	public float CoinHighlightTime;

	public void OnInitializePotentialDrag(PointerEventData data)
	{
		Debug.Log("Init Potential Drag");

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
			   * Canvas.GetComponent<RectTransform>().rect.size;
	}

	private IEnumerator SelectBuildingMenu(Vector3Int mapPosition)
	{
		Vector3 worldPosition = BuildingManager.Map.GetCellCenterWorld(mapPosition);
		Vector3 canvasPosition = WorldToCanvasPosition(worldPosition);
		Debug.Log(canvasPosition);

		BuildingData[] options = BuildingManager.GetRepairOptions(mapPosition);

		GameObject[] coins = new GameObject[options.Length];
		Coroutine[] animations = new Coroutine[options.Length];
		for (int i = 0; i < options.Length; i++)
		{
			coins[i] = Instantiate(SelectionCoinPrefab, Canvas.transform);

			Image previewImage = coins[i].transform.GetChild(0).GetChild(0).GetComponent<Image>();
			previewImage.sprite = options[i].PreviewSprite;
			previewImage.alphaHitTestMinimumThreshold = 0.9f;

			coins[i].GetComponent<RectTransform>().anchoredPosition = canvasPosition;

			animations[i] = StartCoroutine(ExtendCoin(
				coins[i],
				Mathf.Lerp(0, 2 * Mathf.PI, (float)i / options.Length)
			));
		}
		foreach (Coroutine coroutine in animations)
		{
			yield return coroutine;
		}

		for (int i = 0; i < coins.Length; i++)
		{
			SelectionCoinController coinController = coins[i].AddComponent<SelectionCoinController>();
			coinController.MenuController = this;
			coinController.OptionIndex = i;
			coinController.HighlightScale = CoinHighlightScale;
			coinController.HighlightTime = CoinHighlightTime;
		}

		_waitingUserSelection = true;
		SelectedIndex = null;

		yield return new WaitWhile(() => _waitingUserSelection);

		for (int i = 0; i < coins.Length; i++)
		{
			Destroy(coins[i].GetComponent<SelectionCoinController>());

			if (SelectedIndex.HasValue && SelectedIndex.Value == i)
			{
				animations[i] = null;
				continue;
			}
			animations[i] = StartCoroutine(RetractCoin(coins[i], canvasPosition));
		}
		foreach (Coroutine coroutine in animations)
		{
			yield return coroutine;
		}

		if (SelectedIndex.HasValue)
		{
			BuildingManager.RepairTile(mapPosition, options[SelectedIndex.Value]);
			StartCoroutine(RetractCoin(coins[SelectedIndex.Value], coins[SelectedIndex.Value].GetComponent<RectTransform>().anchoredPosition));
		}
	}

	private IEnumerator ExtendCoin(GameObject coin, float angle)
	{
		float startTime = Time.time;

		RectTransform coinTransform = coin.GetComponent<RectTransform>();
		Vector3 startPosition = coinTransform.anchoredPosition;
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

	private IEnumerator RetractCoin(GameObject coin, Vector3 endPosition)
	{
		float startTime = Time.time;

		RectTransform coinTransform = coin.GetComponent<RectTransform>();
		Vector3 startPosition = coinTransform.anchoredPosition;

		while (Time.time - startTime < CoinsExpandTime)
		{
			float progress = (Time.time - startTime) / CoinsExpandTime;

			coinTransform.localScale = Vector3.one * (1 - progress);
			coinTransform.anchoredPosition = Vector3.Lerp(startPosition, endPosition, progress);

			yield return null;
		}
	}

	// For some reason drag events stopped working if the component does not implement IDragHandler
	public void OnDrag(PointerEventData data)
	{ }

	public void OnEndDrag(PointerEventData data)
	{
		_waitingUserSelection = false;
	}
}
