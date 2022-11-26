using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PickupItem : MonoBehaviour {

	[SerializeField] private ClickedItem _clickedItem;
	[SerializeField] private UnityEvent _itemClickEvent;
	[SerializeField] private uint _itemIdentifier;
	[SerializeField] private string _itemCommentary;
	[SerializeField] private TextDictionary _itemComments;
	public Image ItemPrefab;

	// ------ Unity Handlers ------

	public void Awake() {
		_itemComments.textDict[_itemIdentifier] = _itemCommentary;
	}

	public void OnMouseUp() {
		_clickedItem.ItemGameObject = this.gameObject;
		_itemClickEvent.Invoke();
	}

	// ------ Methods ------

	public uint GetItemId() {
		return _itemIdentifier;
	}
}
