using UnityEngine;
using UnityEngine.Events;

public class ActionQueue : MonoBehaviour {
	[SerializeField] private ClickedItem _clickedItem;
	[SerializeField] private MoveTarget _characterMoveTarget;

	[SerializeField] private UnityEvent _ItemPickupEvent;
	[SerializeField] private UnityEvent _moveTargetSetEvent;

	private bool _isMovingToPickupItem = false;

	public void MoveCharacterToClickedItem() {
		_characterMoveTarget.Destination = _clickedItem.ItemGameObject.transform.position;
		_isMovingToPickupItem = true;
		_moveTargetSetEvent.Invoke();
	}

	public void PickupClickedItem() {
		if (_isMovingToPickupItem) {
			_isMovingToPickupItem = false;
			_ItemPickupEvent.Invoke();
		}
	}

	public void CancelItemPickup() {
		_isMovingToPickupItem = false;
	}
}
