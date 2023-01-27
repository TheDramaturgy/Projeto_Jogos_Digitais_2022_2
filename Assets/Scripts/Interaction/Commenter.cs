using System.Collections;
using UnityEngine;
using TMPro;
using static Commentary;

public class Commenter : MonoBehaviour {
	[SerializeField] private TMP_Text _speakText;
	[SerializeField] private PlayerController _character;

	private IEnumerator _lastCommentCoroutine;
	private Commentary _currentCommentary;
	private int _nextCommentIndex = 0;
	private bool _isCommenting = false;

	private void Update() {
		if (_isCommenting) {
			if (Input.anyKey) {
			
				NextComment();
			}
		}
	}

	public void Comment(Commentary commentary) {
		_isCommenting = true;
		if (_character != null) {
			_character.SetControlable(false);
			_character.SetInteractionCapacity(false);
		}

		ClearComment();
		_nextCommentIndex = 0;
		_currentCommentary = commentary;
		NextComment();
	}

	private void NextComment() {
		ClearComment();

		if (_nextCommentIndex >= _currentCommentary.GetCommentariesCount()) {
			EndComment();
			return;
		}

		var dialog = _currentCommentary.IsContinuous() ? _currentCommentary.GetDialog(_nextCommentIndex) : _currentCommentary.GetNextDialog();
		_nextCommentIndex++;

		_lastCommentCoroutine = CharacterComment(dialog.text, dialog.duration);
		StartCoroutine(_lastCommentCoroutine);
	}

	public void ClearComment() {
		if (_lastCommentCoroutine != null) {
			_speakText.text = "";
			StopCoroutine(_lastCommentCoroutine);
		}
	}

	private void EndComment() {
		_isCommenting = false;
		if (_character != null) {
			_character.SetControlable(true);
			_character.SetInteractionCapacity(true);
		}
	}

	private IEnumerator CharacterComment(string text, float duration) {
		_speakText.text = text;
		yield return new WaitForSeconds(duration);
		_speakText.text = "";
		NextComment();
	}
}
