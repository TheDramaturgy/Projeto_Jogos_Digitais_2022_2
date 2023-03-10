using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour {

	[SerializeField] private UnityEvent _onInteractionEvent;
	[SerializeField] private bool _needCharacter = true;
	[SerializeField] private GameObjectVariable _clickedGameObject;
	[SerializeField] private PlayerController _character;
	[SerializeField] private float _interactionRange = 1.0f;
	[SerializeField] private float _xOffset = 0.0f;
	[SerializeField] private bool _isException = false;
	private bool _shouldStartInteraction = false;
	private bool _previousControl;

	private void OnMouseDown() {
		var isMouseOverUI = EventSystem.current.IsPointerOverGameObject();
		if (!isMouseOverUI && GameController.Instance.CanInteract()) {
			GameController.Instance.SetClickingInteractible(true);
			_shouldStartInteraction = true;
		} else _shouldStartInteraction = false;
	}

	private void OnMouseUp() {
		if (!_shouldStartInteraction) { return; }

		GameController.Instance.SetClickingInteractible(false);
		_clickedGameObject.Value = this.gameObject;
		if (_needCharacter) {
			_character.MoveCharacterToClickedItem(_interactionRange, _xOffset, OnInteractableReach, _isException);
		} else {
			OnInteractableReach();
		}
	}

	public void OnInteractableReach() {
		CursorManager.Instance.ActivePlainCursor();
		_onInteractionEvent.Invoke();
	}

	public void AddInteraction(UnityAction action) => _onInteractionEvent.AddListener(action);
	public void SetPlayer(PlayerController player) => _character = player;

}
