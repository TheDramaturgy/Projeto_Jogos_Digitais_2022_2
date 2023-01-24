using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;

public class ItemInteractable : MonoBehaviour, IDropHandler {

	[SerializeField] private UnityEvent _onRightItemDropEvent;
	[SerializeField] private UnityEvent _onWrongItemDropEvent;
	[SerializeField] private GameObjectVariable _clickedGameObject;
	[SerializeField] private PlayerController _character;
	[SerializeField] private float _interactionRange = 1.0f;
	[SerializeField] private int _expectedItemId = -1;
	private GameObject _dropedItem;
	
	public void OnDrop(PointerEventData eventData) {
		_dropedItem = eventData.pointerDrag;
		_clickedGameObject.Value = this.gameObject;
		_character.MoveCharacterToClickedItem(_interactionRange, OnInteractableReach);
	}

	public void OnInteractableReach() {
		var dropedInventoryItem = _dropedItem.GetComponent<InventoryItem>();
		if (dropedInventoryItem.GetId() == _expectedItemId) { _onRightItemDropEvent.Invoke(); }
		else { _onWrongItemDropEvent.Invoke(); }
	}
}
