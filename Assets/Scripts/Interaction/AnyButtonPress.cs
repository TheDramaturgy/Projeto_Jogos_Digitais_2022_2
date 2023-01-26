using UnityEngine;
using UnityEngine.Events;

public class AnyButtonPress : MonoBehaviour {
	[SerializeField] UnityEvent _onAnyButtonPressEvent;

	private void Update() {
		if (Input.anyKey) {
			_onAnyButtonPressEvent.Invoke();
		}
	}
}
