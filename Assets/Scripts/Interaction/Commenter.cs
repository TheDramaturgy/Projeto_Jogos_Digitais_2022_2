using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Commenter : MonoBehaviour {
	[SerializeField] private TMP_Text _speakText;
	[SerializeField] private PlayerController _character;

	private IEnumerator _lastCommentCoroutine;
	private Commentary _currentCommentary;
	private Commentary.Dialog _currentDialog;
	private int _nextCommentIndex = 0;
	private float _writingSpeed = 0.075f;
	private bool _isCommenting = false;
	private bool _isWriting = false;
	private bool _isWaiting = false;
	private bool _previousMovingState;
	private bool _mustEnableControl = false;

	private void Update() {
		if (_isCommenting) {
			if (Input.anyKeyDown && !EventSystem.current.IsPointerOverGameObject()) { NextAction(); }
		}
	}


	public void CommentAndEnableControl(Commentary commentary) {
		if (_isCommenting) {
			StopCoroutine(_lastCommentCoroutine);
			ClearDialog();
			EndComment();
		}

		_mustEnableControl = true;
		_isCommenting = true;
		GameController.Instance.DisableInteraction();
		if (_character != null) {
			_character.SetControlable(false);
		}

		_nextCommentIndex = 0;
		_currentCommentary = commentary;
		NextDialog();
	}

	public void Comment(Commentary commentary) {
		if (_isCommenting) {
			StopCoroutine(_lastCommentCoroutine);
			ClearDialog();
			EndComment();
		}

		_isCommenting = true;
		GameController.Instance.DisableInteraction();
		if (_character != null) {
			_previousMovingState = _character.CanMove();
			_character.SetControlable(false);
		}

		_nextCommentIndex = 0;
		_currentCommentary = commentary;
		NextDialog();
	}

	private void NextDialog() {
		if (_nextCommentIndex >= _currentCommentary.GetCommentariesCount()) {
			EndComment();
			return;
		}

		_currentDialog = _currentCommentary.IsContinuous() ? _currentCommentary.GetDialog(_nextCommentIndex) : _currentCommentary.GetNextDialog();
		_nextCommentIndex++;

		_lastCommentCoroutine = WriteDialogOverTime();
		StartCoroutine(_lastCommentCoroutine);
	}

	public void ClearDialog() {
		_speakText.text = "";
	}

	private void EndComment() {
		GameController.Instance.EnableInteraction();
		if (_character != null) {
			if (_mustEnableControl) {
				_character.SetControlableDelayed(true);
				_mustEnableControl = false;
			} else _character.SetControlableDelayed(_previousMovingState);
		}
		_isCommenting = false;
	}

	private IEnumerator WriteDialogOverTime() {
		_isWriting = true;

		var position = 0;
		while (position < _currentDialog.text.Length) {
			do {
				_speakText.text += _currentDialog.text[position++];
				if (position >= _currentDialog.text.Length) break;
			} while (_currentDialog.text[position] == ' ');
			yield return new WaitForSeconds(_writingSpeed);
		}

		_isWriting = false;

		_lastCommentCoroutine = ClearDialogAfterTime();
		StartCoroutine(_lastCommentCoroutine);
	}

	private IEnumerator ClearDialogAfterTime() {
		_isWaiting = true;

		_speakText.text = _currentDialog.text;
		if (_currentDialog.duration < 0) yield break;
		yield return new WaitForSeconds(_currentDialog.duration);
		ClearDialog();

		_isWaiting = false;
		NextDialog();
	}

	private void NextAction() {
		if (_isWriting) {
			StopCoroutine(_lastCommentCoroutine);
			_isWriting = false;
			_lastCommentCoroutine = ClearDialogAfterTime();
			StartCoroutine(_lastCommentCoroutine);
		} else if (_isWaiting) {
			StopCoroutine(_lastCommentCoroutine);
			_isWaiting = false;
			ClearDialog();
			NextDialog();
		}
	}
}
