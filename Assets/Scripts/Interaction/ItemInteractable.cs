using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ItemInteractable : MonoBehaviour {

	[SerializeField] private UnityEvent _onRightItemDropEvent;
	[SerializeField] private UnityEvent _onWrongItemDropEvent;
	[SerializeField] private bool _needCharacter = true;
	[SerializeField] private GameObjectVariable _clickedGameObject;
	[SerializeField] private PlayerController _character;
	[SerializeField] private int _expectedItemId = -1;
	[SerializeField] private float _interactionRange = 1.0f;
	[SerializeField] private float _xOffset = 0.0f;
	private GameObject _dropedItem;
	
	public void OnDrop(GameObject dropedItem) {
		_dropedItem = dropedItem;
		Debug.Log("Item droped on -> " + this.name);
		if (_needCharacter) {
			_clickedGameObject.Value = this.gameObject;
			_character.MoveCharacterToClickedItem(_interactionRange, _xOffset, OnInteractableReach);
		} else {
			OnInteractableReach();
		}
	}

	public void OnInteractableReach() {
		var dropedInventoryItem = _dropedItem.GetComponent<InventoryItem>();
		if (dropedInventoryItem.GetId() == _expectedItemId) {
			Destroy(_dropedItem);
			_onRightItemDropEvent.Invoke(); 
		} else { _onWrongItemDropEvent.Invoke(); }
	}
}
