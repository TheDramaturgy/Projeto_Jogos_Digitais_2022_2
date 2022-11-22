using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour {

	[Tooltip("Event to register.")]
	public GameEvent Event;
	[Tooltip("Response to event raised.")]
	public UnityEvent Response;

	private void OnEnable() {
		Event.RegisterListener(this);
	}

	private void OnDisable() {
		Event.UnregisterListener(this);
	}

	public void OnEventRaised() {
		Response.Invoke();
	}
}
