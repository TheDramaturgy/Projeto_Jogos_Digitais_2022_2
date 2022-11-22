using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour {

	[SerializeField] private InventorySet _inventorySet;
	[SerializeField] private ClickedItem clickedItem;

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
		if (_inventorySet.GetFreeSlot(out int idx)) {
			var item = Instantiate(clickedItem.ItemGameObject.GetComponent<PickupItem>().ItemPrefab, _slots[idx].transform);
			_inventorySet.AddItemToSlot(item.gameObject, idx);
			Destroy(clickedItem.ItemGameObject);
			clickedItem.Clear();
		}
	}

}
