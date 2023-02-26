using UnityEngine;
using UnityEngine.EventSystems;

public class gameObjectPresenter : MonoBehaviour {
	[SerializeField] private PlayerController _character;

	private GameObject _objectPresented;
	private bool _isPresenting = false;

	void Update() {
		if (_isPresenting) {
			if (Input.anyKeyDown && _isPresenting && !EventSystem.current.IsPointerOverGameObject()) {
				InterruptShowGameObject();
			}
		}
	}

	public void ShowGameObject(GameObject go) {
		_objectPresented = go;
		ActionQueue.Instance.AddAction(TriggerShowGameObject, InterruptShowGameObject);
	}

	public void TriggerShowGameObject() {
		GameController.Instance.DisableInteraction();
		GameController.Instance.DisableCharacterMovement();
		_objectPresented.SetActive(true);
		_isPresenting = true;
	}

	public void InterruptShowGameObject() {
		GameController.Instance.EnableInteractionDelayed();
		GameController.Instance.EnableCharacterMovementDelayed();
		_objectPresented.SetActive(false);
		_isPresenting = false;
		ActionQueue.Instance.NextAction();
	}
}
