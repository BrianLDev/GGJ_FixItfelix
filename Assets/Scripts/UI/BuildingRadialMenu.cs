using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingRadialMenu : MonoBehaviour, IPointerClickHandler
{
	public BuildingManager BuildingManager;
	public Canvas Canvas;
	public GameObject SelectionCoinPrefab;

	private bool _active = false;
	private bool _clickedAgain = false;
	public int? SelectedIndex;

	public float CoinsExpandTime;
	public float CoinNormalRange;
	public float CoinHighlightScale;
	public float CoinHighlightTime;

	public Sprite RepairSprite;
	public Sprite UpgradeHealthSprite;
	public Sprite UpgradeProductionSprite;

	public void OnPointerClick(PointerEventData data)
	{
		if (_active)
		{
			_clickedAgain = true;
			return;
		}

		Vector3Int mapPosition = BuildingManager.Map.WorldToCell(data.pointerCurrentRaycast.worldPosition);

		if (BuildingManager.HasRuin(mapPosition))
		{
			SelectConstructBuildingMenu(mapPosition);
		}
		else if (BuildingManager.HasActiveBuilding(mapPosition))
		{
			SelectBuildingActionMenu(mapPosition);
		}
	}

	private void SelectConstructBuildingMenu(Vector3Int mapPosition)
	{
		StartCoroutine(DoSelectMenu(
			mapPosition,
			() => BuildingManager.GetConstructionOptions(mapPosition),
			data => data.PreviewSprite,
			(index, buildingData) => BuildingManager.ConstructBuildingOnTile(mapPosition, buildingData)
		));
	}

	private void SelectBuildingActionMenu(Vector3Int mapPosition)
	{
		StartCoroutine(DoSelectMenu(
			mapPosition,
			() => BuildingManager.GetBuildingActionOptions(mapPosition),
			action =>
			{
				switch (action)
				{
					case BuildingManager.BuildingAction.REPAIR:
						return RepairSprite;
					case BuildingManager.BuildingAction.UPGRADE_HEALTH:
						return UpgradeHealthSprite;
					case BuildingManager.BuildingAction.UPGRADE_PRODUCTION:
						return UpgradeProductionSprite;
				}
				return null;
			},
			(index, action) => BuildingManager.ExecuteActionOnBuilding(mapPosition, action)
		));
	}

	private Vector2 WorldToCanvasPosition(Vector2 worldPosition)
	{
		return Camera.main.WorldToScreenPoint(worldPosition)
			   / new Vector2(Screen.width, Screen.height)
			   * Canvas.GetComponent<RectTransform>().rect.size;
	}

	private IEnumerator DoSelectMenu<TOption>(
		Vector3Int mapPosition,
		Func<TOption[]> getOptions,
		Func<TOption, Sprite> getPreviewSprite,
		Action<int, TOption> executeSelection
	)
	{
		_active = true;
		_clickedAgain = false;

		Vector3 worldPosition = BuildingManager.Map.GetCellCenterWorld(mapPosition);
		Vector3 canvasPosition = WorldToCanvasPosition(worldPosition);

		TOption[] options = getOptions();

		GameObject[] coins = new GameObject[options.Length];
		Coroutine[] animations = new Coroutine[options.Length];
		for (int i = 0; i < options.Length; i++)
		{
			coins[i] = Instantiate(SelectionCoinPrefab, Canvas.transform);

			Image previewImage = coins[i].transform.GetChild(0).GetChild(0).GetComponent<Image>();
			previewImage.sprite = getPreviewSprite(options[i]);

			coins[i].GetComponent<RectTransform>().anchoredPosition = canvasPosition;

			SelectionCoinController coinController = coins[i].AddComponent<SelectionCoinController>();
			coinController.MenuController = this;
			coinController.OptionIndex = i;
			coinController.HighlightScale = CoinHighlightScale;
			coinController.HighlightTime = CoinHighlightTime;
			coinController.CanScale = false;

			animations[i] = StartCoroutine(ExtendCoin(
				coins[i],
				Mathf.Lerp(0, 2 * Mathf.PI, (float)i / options.Length)
			));
		}
		foreach (Coroutine coroutine in animations)
		{
			yield return coroutine;
		}

		SelectedIndex = null;
		for (int i = 0; i < coins.Length; i++)
		{
			coins[i].GetComponent<SelectionCoinController>().CanScale = true;
		}

		yield return new WaitUntil(() => _clickedAgain || SelectedIndex.HasValue);

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
			int index = SelectedIndex.Value;
			executeSelection(index, options[index]);
			yield return StartCoroutine(RetractCoin(coins[index], coins[index].GetComponent<RectTransform>().anchoredPosition));
		}

		for (int i = 0; i < coins.Length; i++)
		{
			Destroy(coins[i]);
		}

		_active = false;
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
		Vector3 startScale = coinTransform.localScale;

		while (Time.time - startTime < CoinsExpandTime)
		{
			float progress = (Time.time - startTime) / CoinsExpandTime;

			coinTransform.localScale = startScale * (1 - progress);
			coinTransform.anchoredPosition = Vector3.Lerp(startPosition, endPosition, progress);

			yield return null;
		}
	}
}
