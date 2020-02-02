using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionCoinController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	[HideInInspector]
	public BuildingRadialMenu MenuController;
	[HideInInspector]
	public int OptionIndex;
	[HideInInspector]
	public float HighlightScale;
	[HideInInspector]
	public float HighlightTime;
	[HideInInspector]
	public bool CanScale = false;
	private bool _hover;

	private float _currentScale = 1;


	public void OnPointerEnter(PointerEventData data)
	{
		_hover = true;
	}

	private void Update()
	{
		if (!CanScale) return;

		if (_hover)
		{
			_currentScale = Mathf.Min(_currentScale + Time.deltaTime / HighlightTime, HighlightScale);
		}
		else
		{
			_currentScale = Mathf.Max(_currentScale - Time.deltaTime / HighlightTime, 1);
		}

		transform.localScale = Vector3.one * _currentScale;
	}

	public void OnPointerClick(PointerEventData data)
	{
		MenuController.SelectedIndex = OptionIndex;
	}

	public void OnPointerExit(PointerEventData data)
	{
		_hover = false;
	}
}
