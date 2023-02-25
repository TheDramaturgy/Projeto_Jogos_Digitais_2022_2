using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class DropItemFromMind : MonoBehaviour {
	[SerializeField] Transform _startPosition;
	[SerializeField] Transform _endPosition;
	private Queue<GameObject> _itemsToDrop = new();

	[Header("Pickup Items")]
	[SerializeField] InventoryController _inventory;

	public void DropItem(GameObject prefab) {
		_itemsToDrop.Enqueue(prefab);
		PlayerActionQueue.Instance.AddAction(TriggerDropItem);
	}

	public void TriggerDropItem() {
		var prefab = _itemsToDrop.Dequeue();
		var item = Instantiate(prefab, _startPosition.position, Quaternion.identity, transform.parent);
		var interactable = item.GetComponent<Interactable>();

		interactable.AddInteraction(_inventory.PickupClickedItem);
		interactable.SetPlayer(transform.GetComponent<PlayerController>());

		item.transform.DOJump(_endPosition.position, 1.5f, 1, 2.0f).OnComplete(() => {
			PlayerActionQueue.Instance.NextAction();
		});
	}
}
