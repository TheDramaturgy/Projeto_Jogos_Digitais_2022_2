using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour {
	[SerializeField] private List<Image> _imageComponents = new();
	[SerializeField] private List<TMP_Text> _textComponents = new();
	[SerializeField] private GameObject _btnSair;
	private float _animationSpeed = 1.0f;

	private void Start() {
		DisableCredits();
	}

	public void EnableCredits() {
		foreach (var image in _imageComponents) {
			image.gameObject.SetActive(true);
			DOTween.Kill(image);
			image.DOFade(1.0f, _animationSpeed);
		}

		StartCoroutine(ActivateTexts());
	}

	private IEnumerator ActivateTexts() {
		foreach (var text in _textComponents) {
			text.gameObject.SetActive(true);
			DOTween.Kill(text);
			var tween = text.DOFade(1.0f, _animationSpeed);
			while (!tween.IsComplete()) yield return null;
		}

		_btnSair.SetActive(true);
	}

	private void DisableCredits() {
		foreach (var text in _textComponents) {
			DOTween.Kill(text);
			text.DOFade(0.0f, _animationSpeed);
		}

		foreach (var image in _imageComponents) {
			DOTween.Kill(image);
			image.DOFade(0.0f, _animationSpeed).OnComplete(() => { image.gameObject.SetActive(false); });
		}
	}

}
