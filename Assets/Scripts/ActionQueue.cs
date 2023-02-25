using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionQueue : MonoBehaviour {
	private Queue<UnityAction> _actionQueue = new();
	private Queue<UnityAction> _interruptionQueue = new();
	private UnityAction _currentAction;
	private UnityAction _currentInterruption;
	private bool _isActing = false;

	public static ActionQueue Instance { get; private set; }

	private void Awake() {
		if (Instance != null && Instance != this) Destroy(this);
		else Instance = this;
	}

	public void AddAction(UnityAction action, UnityAction interruption = null) {
		_actionQueue.Enqueue(action);
		if (interruption != null) _interruptionQueue.Enqueue(interruption);
		else _interruptionQueue.Enqueue(EmptyInterruption);

		CheckQueueExecution();
	}

	public void NextAction() {
		_isActing = false;
		CheckQueueExecution();
	}

	public void InterruptCurrentAction() {
		if (_currentInterruption != null) {
			_currentInterruption.Invoke();
		}
	}

	public void ClearAllActions() {
		while (_actionQueue.Count > 0 || _currentAction != null) {
			InterruptCurrentAction();
		}
	}

	private IEnumerator InterruptActionsAsync() {
		while (_actionQueue.Count > 0 || _currentAction != null) {
			InterruptCurrentAction();
			yield return new WaitForSeconds(Time.deltaTime * 2);
		}
	}

	private void CheckQueueExecution() {
		if (_isActing) {
			return;
		} else if (_actionQueue.Count <= 0) {
			_currentAction = null;
			_currentInterruption = null;
			return;
		}

		_isActing = true;

		_currentAction = _actionQueue.Dequeue();
		_currentInterruption = _interruptionQueue.Dequeue();
		_currentAction.Invoke();
	}

	private void EmptyInterruption() {
		return;
	}
}
