using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionCoinController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[HideInInspector]
	public BuildingRadialMenu MenuController;
	[HideInInspector]
	public int OptionIndex;
	[HideInInspector]
	public float HighlightScale;
	[HideInInspector]
	public float HighlightTime;
	private bool _hover;

	private float _currentScale = 1;
	

	public void OnPointerEnter(PointerEventData data)
	{
		MenuController.SelectedIndex = OptionIndex;
		_hover = true;
	}

	private void Update()
	{
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

	public void OnPointerExit(PointerEventData data)
	{
		if (MenuController.SelectedIndex == OptionIndex)
		{
			MenuController.SelectedIndex = null;
		}
		_hover = false;
	}
}
