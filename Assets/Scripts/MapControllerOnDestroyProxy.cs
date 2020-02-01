using UnityEngine;
using UnityEngine.Events;

public class MapControllerOnDestroyProxy : MonoBehaviour
{
	[HideInInspector]
	public UnityEvent OnDestroyEvent = new UnityEvent();

	private void OnDestroy()
	{
		OnDestroyEvent.Invoke();
	}
}
