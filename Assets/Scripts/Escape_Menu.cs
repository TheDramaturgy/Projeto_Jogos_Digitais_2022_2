using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Escape_Menu : MonoBehaviour {
	[SerializeField] private List<Image> _imageComponents = new();
	[SerializeField] private List<TMP_Text> _textComponents = new();
	private bool _isActive = false;
	private float _animationSpeed = 0.3f;

	private void Start() {
		DisableEscapeMenu();
	}

	private void Update() {
		OnEscapeClick();
	}

	private void OnEscapeClick() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (_isActive) {
				DisableEscapeMenu();
				_isActive = false;
			} else {
				EnableEscapeMenu();
				_isActive = true;
			}
		}
	}

	private void DisableEscapeMenu() {
		foreach (var text in _textComponents) {
			DOTween.Kill(text);
			text.DOFade(0.0f, _animationSpeed);
		}

		foreach (var image in _imageComponents) {
			DOTween.Kill(image);
			image.DOFade(0.0f, _animationSpeed).OnComplete(() => { image.gameObject.SetActive(false); });
		}
	}

	private void EnableEscapeMenu() {
		foreach (var image in _imageComponents) {
			image.gameObject.SetActive(true);
			DOTween.Kill(image);
			image.DOFade(1.0f, _animationSpeed);
		}

		foreach (var text in _textComponents) {
			DOTween.Kill(text);
			text.DOFade(1.0f, _animationSpeed);
		}
	}
}
