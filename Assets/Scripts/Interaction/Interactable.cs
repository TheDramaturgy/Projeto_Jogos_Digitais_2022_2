using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Interactable : MonoBehaviour {

	[SerializeField] private UnityEvent _onInteractionEvent;
	[SerializeField] private bool _needCharacter = true;
	[SerializeField] private GameObjectVariable _clickedGameObject;
	[SerializeField] private PlayerController _character;
	[SerializeField] private float _interactionRange = 1.0f;
	[SerializeField] private float _xOffset = 0.0f;
	[SerializeField] private bool _isMouseOveUI = false;
	private bool _shouldStartInteraction = false;

	// ------ Unity Handlers ------

	private void Update() {
		_isMouseOveUI = EventSystem.current.IsPointerOverGameObject();
	}

	private void OnMouseDown() {
		if (!_isMouseOveUI) _shouldStartInteraction = true;
	}

	private void OnMouseUp() {
		if (_isMouseOveUI)
        {
			Debug.Log("Mouse is over UI");
			return;
		}	
			
		if (!_shouldStartInteraction) {
			Debug.Log("Should Not Start Interaction");
			return;
		}

		Debug.Log("Interacted with -> " + this.name);
		if (_needCharacter) {
			_clickedGameObject.Value = this.gameObject;
			_character.MoveCharacterToClickedItem(_interactionRange, _xOffset, OnInteractableReach);
		} else {
			OnInteractableReach();
		}
	}

	public void OnInteractableReach() {
		_onInteractionEvent.Invoke();
	}
}
