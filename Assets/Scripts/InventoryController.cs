using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class InventoryController : MonoBehaviour {

	[SerializeField] private UnityEvent _notEnoughIventorySlotsEvent;
	[SerializeField] private UnityEvent _inventoryChangeEvent;

	[SerializeField] private InventorySet _inventorySet;
	[SerializeField] private GameObjectVariable clickedItem;

	private readonly List<GameObject> _slots = new();

	// ------ Unity Handlers ------

	void Awake() {
		for (int i = 0; i < this.transform.childCount; ++i) {
			var currentSlot = this.transform.GetChild(i).gameObject;
			currentSlot.GetComponent<InventorySlot>().SetIndex(i);
			_slots.Add(currentSlot);
		}
	}


	// ------ Event Handlers ------

	public void AddItem() {
		var clickedPickup = clickedItem.Value.GetComponent<PickupItem>();

		if (_inventorySet.GetFreeSlot(clickedPickup.GetPickupQuantity(), out List<int> idx)) {
			for (int i = 0; i < idx.Count; ++i) {
				var item = Instantiate(clickedPickup.ItemPrefab, _slots[idx[i]].transform);
				_inventorySet.AddItemToSlot(item.gameObject, idx[i]);
			}
			clickedItem.Value.SetActive(false);
			_inventoryChangeEvent.Invoke();
		} else {
			Debug.Log("Not enough inventory slots.");
			_notEnoughIventorySlotsEvent.Invoke();
		}
	}

	public void ChangeItemSlot(GameObject movingItemGO, int newSlotIdx) {
		var movingItem = movingItemGO.GetComponent<InventoryItem>();

		// If slot not ocupied, move draged item to slot
		// Else, swap item slots
		if (!_inventorySet.isOccupied(newSlotIdx)) {
			movingItem.ChangeParent(_slots[newSlotIdx].transform);
			_inventorySet.RemoveItem(movingItemGO);
			_inventorySet.AddItemToSlot(movingItemGO, newSlotIdx);
		} else {
			var occupyingItem = _inventorySet.GetItemFromSlot(newSlotIdx).GetComponent<InventoryItem>();
			var oldSlotIdx = movingItem.GetCurrentParent().GetComponent<InventorySlot>().GetIndex();

			// Swap Transform Parents
			occupyingItem.ChangeParent(movingItem.GetCurrentParent());
			movingItem.ChangeParent(_slots[newSlotIdx].transform);

			// Swap items in InventorySet
			_inventorySet.SwapItemsBetweenSlots(oldSlotIdx, newSlotIdx);
		}

		_inventoryChangeEvent.Invoke();
	}

}
