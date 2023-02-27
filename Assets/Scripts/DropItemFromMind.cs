using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DropItemFromMind : MonoBehaviour {
	[SerializeField] UnityEvent _OnFirstTimeDrop;
	[SerializeField] Transform _startPosition;
	[SerializeField] Transform _endPosition;
	private Queue<GameObject> _itemsToDrop = new();

	private bool _firstTime = true;

	[Header("Pickup Items")]
	[SerializeField] InventoryController _inventory;

	public void DropItem(GameObject prefab) {
		_itemsToDrop.Enqueue(prefab);
		ActionQueue.Instance.AddAction(TriggerDropItem);
	}

	public void TriggerDropItem() {
		var prefab = _itemsToDrop.Dequeue();
		var item = Instantiate(prefab, _startPosition.position, Quaternion.identity, transform.parent);
		var interactable = item.GetComponent<Interactable>();

		interactable.AddInteraction(_inventory.PickupClickedItem);
		interactable.SetPlayer(transform.GetComponent<PlayerController>());

		GameController.Instance.DisableInteraction();
		GameController.Instance.DisableCharacterMovement();

		item.transform.DOJump(_endPosition.position, 1.5f, 1, 2.0f).OnComplete(() => {
			GameController.Instance.EnableInteraction();
			GameController.Instance.EnableCharacterMovement();
			ActionQueue.Instance.NextAction();
		});

		if (_firstTime) {
			_firstTime = false;
			_OnFirstTimeDrop.Invoke();
		}
	}
}
