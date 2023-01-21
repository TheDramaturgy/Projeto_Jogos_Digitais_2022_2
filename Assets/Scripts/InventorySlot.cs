using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler {

	[SerializeField] private int _slotIndex;
	private InventoryController _inventoryController;

	// ------ Unity Handlers ------

	public void Awake() {
		_inventoryController = transform.parent.GetComponent<InventoryController>();
	}

	public void OnDrop(PointerEventData eventData) {
		_inventoryController.ChangeItemSlot(eventData.pointerDrag, _slotIndex);
	}

	public void SetIndex(int idx) {
		_slotIndex = idx;
	}

	public int GetIndex() {
		return _slotIndex;
	}
}
