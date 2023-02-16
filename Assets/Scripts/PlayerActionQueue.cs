using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
		Debug.Log("Check Queue");
		if (_isActing || _actionQueue.Count <= 0) return;
		
		_isActing = true;
		var action = _actionQueue.Dequeue();
		Debug.Log("Executing -> " + action.Target.ToString());
		action.Invoke();
	}
}
