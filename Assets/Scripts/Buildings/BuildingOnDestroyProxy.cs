using UnityEngine;
using UnityEngine.Events;

public class BuildingOnDestroyProxy : MonoBehaviour
{
	[HideInInspector]
	public UnityEvent OnDestroyEvent = new UnityEvent();

	private void OnDestroy()
	{
		OnDestroyEvent.Invoke();
	}
}
