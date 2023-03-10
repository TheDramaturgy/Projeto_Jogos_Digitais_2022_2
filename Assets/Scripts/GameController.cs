using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour {
	private bool _isMovingInventoryItem = false;
	private bool _isClickingInteractible = false;

	public static GameController Instance { get; private set; }

	private void Awake() {
		if (Instance != null && Instance != this) Destroy(this);
		else Instance = this;
	}

	public bool CanInteract() {
		return !ActionQueue.Instance.IsBlockedByAction();
	}

	public bool CanMove() {
		return (!ActionQueue.Instance.IsBlockedByAction() && !_isMovingInventoryItem && !_isClickingInteractible);
	}

	public void SetMovingInventoryItem(bool isMoving) => _isMovingInventoryItem = isMoving;
	public void SetClickingInteractible(bool isClicking) => _isClickingInteractible = isClicking;
}
