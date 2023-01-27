using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemInteractable : MonoBehaviour {

	[SerializeField] private UnityEvent _onRightItemDropEvent;
	[SerializeField] private UnityEvent _onWrongItemDropEvent;
	[SerializeField] private bool _needCharacter = true;
	[SerializeField] private GameObjectVariable _clickedGameObject;
	[SerializeField] private PlayerController _character;
	[SerializeField] private List<int> _expectedItemId = new();
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

		for (int i = 0; i < _expectedItemId.Count; i++) {
			if (dropedInventoryItem.GetId() == _expectedItemId[i]) {
				Destroy(_dropedItem);
				_onRightItemDropEvent.Invoke();
				return;
			}
		} 
		_onWrongItemDropEvent.Invoke();
	}
}
