using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayedEvent : MonoBehaviour {
	[SerializeField] private UnityEvent _onDelayReach;
	[SerializeField] private List<BoolVariable> _conditions;
	[SerializeField] private float _delaySeconds = 1.0f;

	public void TriggerDelayedEvent() {
		if (_conditions.Count > 0) {
			foreach (var condition in _conditions) {
				if (!condition) return;
			}
		}
		Debug.Log("Disparing Delayed Action");
		StartCoroutine(StartDelayedEvent());
	}

	private IEnumerator StartDelayedEvent() {
		yield return new WaitForSeconds(_delaySeconds);
		_onDelayReach.Invoke();
	}
}
