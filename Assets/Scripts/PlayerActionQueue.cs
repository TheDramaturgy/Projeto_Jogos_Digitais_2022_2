using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerActionQueue : MonoBehaviour {
	private Queue<UnityAction> _actionQueue = new();
	private bool _isActing = false;
	private bool _mustEnableControl = false;

	public static PlayerActionQueue Instance { get; private set; }

	private void Awake() {
		if (Instance != null && Instance != this) Destroy(this);
		else Instance = this;
	}

	public void AddAction(UnityAction action) {
		_actionQueue.Enqueue(action);
		CheckQueueExecution();
	}

	public void NextAction() {
		_isActing = false;
		CheckQueueExecution();
	}

	private void CheckQueueExecution() {
		if (_isActing) {
			return;
		} else if (_actionQueue.Count <= 0) {
			GameController.Instance.EnableInteractionDelayed();
			if (_mustEnableControl) GameController.Instance.EnableCharacterMovementDelayed();
			else {
				GameController.Instance.DisableCharacterMovement();
				_mustEnableControl = true;
			}
			return;
		}
		
		_isActing = true;
		GameController.Instance.DisableInteraction();
		GameController.Instance.DisableCharacterMovement();

		var action = _actionQueue.Dequeue();
		action.Invoke();
	}

	public void MustEnableControl() => _mustEnableControl = true;
	public void MustDisableControl() => _mustEnableControl = false;
}
