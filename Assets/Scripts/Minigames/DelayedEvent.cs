using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayedEvent : MonoBehaviour {
	[SerializeField] private UnityEvent _onDelayReach;
	[SerializeField] private UnityEvent _onCoditionNotMet;
	[SerializeField] private List<BoolVariable> _conditions;
	[SerializeField] private float _delaySeconds = 1.0f;

	private Coroutine _delayedAction;

	public void TriggerDelayedEvent() {
		foreach (var condition in _conditions) {
			if (!condition.Value) {
				_onCoditionNotMet.Invoke();
				return;
			}
		}

		if (gameObject.activeSelf) {
			_delayedAction = StartCoroutine(StartDelayedEvent());
		}
	}

	private IEnumerator StartDelayedEvent() {
		yield return new WaitForSeconds(_delaySeconds);
		_onDelayReach.Invoke();
	}

	public void CancelDelayedEvent() {
		if (_delayedAction != null) StopCoroutine(_delayedAction);
	}
}
