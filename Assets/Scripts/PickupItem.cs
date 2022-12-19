using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PickupItem : MonoBehaviour {

	[SerializeField] private UnityEvent _itemClickEvent;
	[SerializeField] private UnityEvent _itemReachedEvent;

	[SerializeField] private GameObjectVariable _clickedItem;
	[SerializeField] private uint _itemIdentifier;
	[SerializeField] private string _itemCommentary;
	[SerializeField] private TextDictionary _itemComments;
	[SerializeField] private uint _pickupQuantity;
	public Image ItemPrefab;

	// ------ Unity Handlers ------

	public void Awake() {
		_itemComments.textDict[_itemIdentifier] = _itemCommentary;
		ItemPrefab.GetComponent<InventoryItem>().SetId(_itemIdentifier);
	}

	public void OnMouseUp() {
		_clickedItem.Value = this.gameObject;
		_itemClickEvent.Invoke();
	}

	private void OnTriggerEnter(Collider other) {
		_itemReachedEvent.Invoke();
	}

	// ------ Methods ------

	public uint GetItemId() {
		return _itemIdentifier;
	}

	public uint GetPickupQuantity() {
		return _pickupQuantity;
	}
}
