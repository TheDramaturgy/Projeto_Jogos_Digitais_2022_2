using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionQueue : MonoBehaviour {
	private struct GameAction {
		public UnityAction action;
		public UnityAction interruption;
		public bool isBlockingAction;

		public GameAction(UnityAction action, UnityAction interruption, bool isBlockingAction) {
			this.action = action;
			this.interruption = interruption;
			this.isBlockingAction = isBlockingAction;
		}
	}

	private Queue<GameAction> _gameActionQueue = new();
	private GameAction _currentGameAction;

	private bool _isActing = false;
	private bool _isBlockedByAction = false;

	public static ActionQueue Instance { get; private set; }

	private void Awake() {
		if (Instance != null && Instance != this) Destroy(this);
		else Instance = this;
	}

	public void AddAction(UnityAction action, UnityAction interruption = null, bool isBlockingAction = false) {
		if (interruption != null) _gameActionQueue.Enqueue(new GameAction(action, interruption, isBlockingAction));
		else _gameActionQueue.Enqueue(new GameAction(action, EmptyInterruption, isBlockingAction));

		CheckQueueExecution();
	}

	public void NextAction() {
		_isActing = false;
		_isBlockedByAction = false;
		CheckQueueExecution();
	}

	public void InterruptCurrentAction() {
		if (_currentGameAction.interruption != null) {
			_currentGameAction.interruption.Invoke();
		}
	}

	public void ClearAllActions() {
		while (_gameActionQueue.Count > 0 || !_currentGameAction.Equals(default(GameAction))) {
			InterruptCurrentAction();
		}
	}

	private IEnumerator InterruptActionsAsync() {
		while (_gameActionQueue.Count > 0 || !_currentGameAction.Equals(default(GameAction))) {
			InterruptCurrentAction();
			yield return new WaitForSeconds(Time.deltaTime * 2);
		}
	}

	private void CheckQueueExecution() {
		if (_isActing) {
			return;
		} else if (_gameActionQueue.Count <= 0) {
			_currentGameAction = default(GameAction);
			return;
		}

		_currentGameAction = _gameActionQueue.Dequeue();
		_isActing = true;
		_isBlockedByAction = _currentGameAction.isBlockingAction; 
		_currentGameAction.action.Invoke();
	}

	private void EmptyInterruption() { return; }

	public bool IsBlockedByAction() => _isBlockedByAction;
}
