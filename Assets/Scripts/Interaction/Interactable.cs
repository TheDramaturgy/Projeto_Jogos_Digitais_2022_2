using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {

	[SerializeField] private UnityEvent _onInteractionEvent;
	[SerializeField] private GameObjectVariable _clickedGameObject;
	[SerializeField] private PlayerController _character;
	[SerializeField] private float _interactionRange = 1.0f;
	private Renderer _renderer;

	// ------ Unity Handlers ------

	private void Start () {
		_renderer = gameObject.GetComponent<Renderer>();
	}

	private void OnMouseUp() {
		_clickedGameObject.Value = this.gameObject;
		_character.MoveCharacterToClickedItem(_interactionRange, OnInteractableReach);
	}

	private void OnMouseEnter() {
		if (GetComponent<Renderer>() != null) {
			GetComponent<Renderer>().material.SetColor("_Color", new Vector4(1.988f, 0.438f, 0.438f, 1.0f));
		}
	}

	private void OnMouseExit() {
		if (GetComponent<Renderer>() != null) {
			GetComponent<Renderer>().material.SetColor("_Color", new Vector4(2.2f, 2.2f, 0.0f, 1.0f));
		}
	}

	public void OnInteractableReach() {
		_onInteractionEvent.Invoke();
	}
}
