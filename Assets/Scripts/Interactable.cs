using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {

	[SerializeField] private UnityEvent _onInteractionEvent;
	[SerializeField] private GameObjectVariable _clickedItem;
	[SerializeField] private PlayerController _character;
	[SerializeField] private float _interactionRange = 1.0f;

	// ------ Unity Handlers ------

	private void OnMouseUp() {
		_clickedItem.Value = this.gameObject;
		_character.MoveCharacterToClickedItem(_interactionRange, OnInteractableReach);
	}

	public void OnInteractableReach() {
		_onInteractionEvent.Invoke();
	}
}
