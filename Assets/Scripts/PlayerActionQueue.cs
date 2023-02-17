using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

public class PlayerActionQueue : MonoBehaviour {
	private Queue<UnityAction> _actionQueue = new();
	private bool _isActing = false;

	public static PlayerActionQueue Instance { get; private set; }

	private void Awake() {
		if (Instance != null && Instance != this) Destroy(this);
		else Instance = this;
	}

	public void AddAction(UnityAction action) {
		Debug.Log(action.Target.ToString() + " being added to the queue.");
		_actionQueue.Enqueue(action);
		CheckQueueExecution();
	}

	public void NextAction() {
		Debug.Log("Next Action Called.");
		_isActing = false;
		CheckQueueExecution();
	}

	private void CheckQueueExecution() {
		if (_isActing) {
			return;
		} else if (_actionQueue.Count <= 0) {
			EnableControl();
			return;
		}
		
		_isActing = true;
		DisableControl();
		var action = _actionQueue.Dequeue();
		action.Invoke();
	}

	private void DisableControl() {
		GameController.Instance.DisableInteraction();
		GameController.Instance.DisableCharacterMovement();
	}

	private void EnableControl() {
		GameController.Instance.EnableInteraction();
		GameController.Instance.EnableCharacterMovement();
	}
}
