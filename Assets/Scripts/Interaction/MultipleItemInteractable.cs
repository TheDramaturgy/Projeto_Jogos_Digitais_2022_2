using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultipleItemInteractable : MonoBehaviour {
	[SerializeField] private UnityEvent _onWrongItemDropEvent;
	[SerializeField] private UnityEvent _onAllItemsCollected;
	[SerializeField] private GameObjectVariable _clickedGameObject;
	[SerializeField] private PlayerController _character;
	[SerializeField] private bool _needCharacter = true;
	[SerializeField] private float _interactionRange = 1.0f;
	[SerializeField] private float _xOffset = 0.0f;

	[Header("--------------- Expected Items ---------------")]
	[SerializeField] private List<int> _expectedItemIds = new();
	[SerializeField] private List<UnityEvent> _onRightItemDropEvents = new();
	

	private GameObject _dropedItem;
	private List<bool> _itemsCollectionStatus = new();

	private void Awake() {
		for (int i = 0; i < _expectedItemIds.Count; i++) {
			_itemsCollectionStatus.Add(false);
		}
	}

	private void OnValidate() {
		while (_expectedItemIds.Count > _onRightItemDropEvents.Count)
			_onRightItemDropEvents.Add(new UnityEvent());
		while (_expectedItemIds.Count < _onRightItemDropEvents.Count)
			_onRightItemDropEvents.RemoveAt(_onRightItemDropEvents.Count-1);
	}

	public void OnDrop(GameObject droppedItem) {
		if (GameController.Instance.CanInteract()) {
			_dropedItem = droppedItem;

			if (_needCharacter) {
				_clickedGameObject.Value = this.gameObject;
				_character.MoveCharacterToClickedItem(_interactionRange, _xOffset, OnInteractableReach);
			} else {
				OnInteractableReach();
			}
		}
	}

	public void OnInteractableReach() {
		var dropedInventoryItem = _dropedItem.GetComponent<InventoryItem>();

		for (int i = 0; i < _expectedItemIds.Count; i++) {
			if (dropedInventoryItem.GetId() == _expectedItemIds[i]) {
				dropedInventoryItem.GetInventoryController().RemoveItem(_dropedItem);
				Destroy(_dropedItem);
				_onRightItemDropEvents[i].Invoke();
				_itemsCollectionStatus[i] = true;
				CheckAllItemsCollectionStatus();
				return;
			}
		}
		_onWrongItemDropEvent.Invoke();
	}

	private void CheckAllItemsCollectionStatus() {
		foreach (var status in _itemsCollectionStatus) {
			if (!status) return;
		}

		_onAllItemsCollected.Invoke();
	}
}
