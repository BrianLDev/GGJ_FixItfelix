using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionCoinController : TooltipListener, IPointerClickHandler
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
	public bool FullyActive = false; // If the coin is fully extended and the player can afford to select this option
	public bool CanAfford;
	private bool _hover;

	private float _currentScale = 1;

    private AudioManagerScript audioManager;

    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();
    }

    public override void OnPointerEnter(PointerEventData data)
	{
        base.OnPointerEnter(data);
        audioManager.PlaySwitchCoin();
        _hover = true;
	}

	private void Update()
	{
		if (!FullyActive) return;

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
		if (!FullyActive) return;
		MenuController.SelectedIndex = OptionIndex;
        Tooltip.HideTooltip();
	}

	public override void OnPointerExit(PointerEventData data)
	{
        base.OnPointerExit(data);
        _hover = false;
	}
}
