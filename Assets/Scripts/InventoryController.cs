using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InventoryController : MonoBehaviour {

	[SerializeField] private UnityEvent _notEnoughIventorySlotsEvent;
	[SerializeField] private UnityEvent _inventoryChangeEvent;

	[SerializeField] private InventorySet _inventorySet;
	[SerializeField] private GameObjectVariable clickedGameObject;

	private readonly List<GameObject> _slots = new();

	// ------ Unity Handlers ------

	void Awake() {
		for (int i = 0; i < this.transform.childCount; ++i) {
			var currentSlot = this.transform.GetChild(i).gameObject;
			currentSlot.GetComponent<InventorySlot>().SetIndex(i);
			_slots.Add(currentSlot);

			if (currentSlot.transform.childCount > 0) {
				var item = currentSlot.transform.GetChild(0);
				_inventorySet.AddItemToSlot(item.gameObject, i);
			}
		}
	}


	// ------ Event Handlers ------

	public void PickupClickedItem() {
		var clickedPickup = clickedGameObject.Value.GetComponent<PickupItem>();

		// Check whether clicked GameObject have the component PickupItem
		if (clickedPickup == null) {
			Debug.LogWarning(clickedGameObject.Value.name + " is not a PickupItem.");
		}

		// Get a number of free slots necessary for item picked up
		if (_inventorySet.GetFreeSlot(clickedPickup.GetPickupQuantity(), out List<int> idx)) {
			for (int i = 0; i < idx.Count; ++i) {
				var item = Instantiate(clickedPickup.ItemPrefab, _slots[idx[i]].transform);
				item.GetComponent<InventoryItem>().SetId(clickedPickup.GetItemId());
				_inventorySet.AddItemToSlot(item.gameObject, idx[i]);
			}

			// Disable picked up item GameObject
			clickedGameObject.Value.SetActive(false);
			_inventoryChangeEvent.Invoke();
		} else {
			Debug.Log("Not enough free inventory slots.");
			_notEnoughIventorySlotsEvent.Invoke();
		}
	}

	public void ChangeItemSlot(GameObject movingItemGO, int newSlotIdx) {
		var movingItem = movingItemGO.GetComponent<InventoryItem>();
		movingItem.ChangeParent(_slots[newSlotIdx].transform);

		// If slot not ocupied, move draged item to slot
		// Else, swap item slots
		if (!_inventorySet.isOccupied(newSlotIdx)) {
			_inventorySet.RemoveItem(movingItemGO);
			_inventorySet.AddItemToSlot(movingItemGO, newSlotIdx);
		} else {
			var occupyingItem = _inventorySet.GetItemFromSlot(newSlotIdx).GetComponent<InventoryItem>();
			var oldSlotIdx = movingItem.GetPreviousParent().gameObject.GetComponent<InventorySlot>().GetIndex();

			// Swap Transform Parents
			occupyingItem.ChangeParent(_slots[oldSlotIdx].transform);
			occupyingItem.UpdatePosition();
			movingItem.ChangeParent(_slots[newSlotIdx].transform);

			// Swap items in InventorySet
			_inventorySet.SwapItemsBetweenSlots(oldSlotIdx, newSlotIdx);
		}

		_inventoryChangeEvent.Invoke();
	}

}
