using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Commenter : MonoBehaviour {
	[SerializeField] private TMP_Text _speakText;
	[SerializeField] private PlayerController _character;

	private IEnumerator _lastCommentCoroutine;
	private Queue<Commentary> _commentaries = new();
	private Commentary _currentCommentary;
	private Commentary.Dialog _currentDialog;
	private int _nextCommentIndex = 0;
	private float _writingSpeed = 0.075f;
	private bool _isCommenting = false;
	private bool _isWriting = false;
	private bool _isWaiting = false;

	private void Update() {
		if (_isCommenting) {
			if (Input.anyKeyDown && !EventSystem.current.IsPointerOverGameObject()) { NextAction(); }
		}
	}

	public void Comment(Commentary commentary) {
		_commentaries.Enqueue(commentary);
		ActionQueue.Instance.AddAction(TriggerComment, InterruptDialog, isBlockingAction: true);
	}

	public void TriggerComment() {
		if (_isCommenting) {
			StopCoroutine(_lastCommentCoroutine);
			ClearDialog();
		} else {
			_currentCommentary = _commentaries.Dequeue();
		}

		_isCommenting = true;
		_nextCommentIndex = 0;
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

	public void InterruptDialog() {
		if (_lastCommentCoroutine != null) StopCoroutine(_lastCommentCoroutine);
		ClearDialog();
		EndComment();
	}

	public void ClearDialog() {
		_speakText.text = "";
	}

	private void EndComment() {
		_isCommenting = false;
		_currentCommentary = null;
		ActionQueue.Instance.NextAction();
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
