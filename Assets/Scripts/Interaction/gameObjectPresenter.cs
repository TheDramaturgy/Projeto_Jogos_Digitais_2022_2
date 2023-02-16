using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class gameObjectPresenter : MonoBehaviour {
	[SerializeField] private PlayerController _character;

	private GameObject _objectPresented;
	private bool _isPresenting = false;

	void Update() {
		if (_isPresenting) {
			if (Input.anyKeyDown && !EventSystem.current.IsPointerOverGameObject()) {
				_objectPresented.SetActive(false);
				GameController.Instance.SetInteractionDelayed(true);
				_character.SetControlableDelayed(true);
				PlayerActionQueue.Instance.NextAction();
			}
		}
	}

	public void ShowGameObject(GameObject go) {
		_objectPresented = go;
		PlayerActionQueue.Instance.AddAction(TriggerShowGameObject);
	}

	public void TriggerShowGameObject() {
		GameController.Instance.DisableInteraction();
		_character.SetControlable(false);

		_objectPresented.SetActive(true);
		_isPresenting = true;
	}
}
