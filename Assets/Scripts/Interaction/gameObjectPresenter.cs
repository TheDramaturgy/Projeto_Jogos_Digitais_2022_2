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
				GameController.Instance.EnableInteraction();
				_character.SetControlableDelayed(true);
			}
		}
	}

	public void ShowGameObject(GameObject go) {
		GameController.Instance.DisableInteraction();
		_character.SetControlable(false);

		_objectPresented = go;
		go.SetActive(true);

		_isPresenting = true;
	}
}
