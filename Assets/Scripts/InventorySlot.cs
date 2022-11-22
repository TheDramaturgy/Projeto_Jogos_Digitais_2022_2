using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler {

	[SerializeField] private InventorySet _inventorySet;
	[SerializeField] private int _slotIndex;

	// ------ Unity Handlers ------

	public void OnDrop(PointerEventData eventData) {
		Debug.Log("Checking Slot " + _slotIndex);
		if (!_inventorySet.isOcupied(_slotIndex)) {
			eventData.pointerDrag.GetComponent<InventoryItem>().ChangeParent(this.transform);
			_inventorySet.RemoveItem(eventData.pointerDrag);
			_inventorySet.AddItemToSlot(eventData.pointerDrag, _slotIndex);
		}
	}

	public void SetIndex(int idx) {
		_slotIndex = idx;
	}
}
