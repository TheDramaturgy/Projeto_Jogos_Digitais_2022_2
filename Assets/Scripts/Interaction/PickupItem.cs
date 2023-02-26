using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PickupItem : MonoBehaviour {
	[SerializeField] private uint _itemIdentifier;
	[SerializeField] private uint _pickupQuantity;
	public Image ItemPrefab;
	public bool IsPickedUp;


	// ------ Methods ------

	public uint GetItemId() {
		return _itemIdentifier;
	}

	public uint GetPickupQuantity() {
		return _pickupQuantity;
	}
}
