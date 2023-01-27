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

	// ------ Unity Handlers ------

	private void OnMouseUp() {
		if (EventSystem.current.IsPointerOverGameObject())
        {
			Debug.Log("Teste Log");
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
