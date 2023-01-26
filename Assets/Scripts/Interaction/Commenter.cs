using System.Collections;
using UnityEngine;
using TMPro;

public class Commenter : MonoBehaviour {
	[SerializeField] private TMP_Text _speakText;
	private IEnumerator _lastCommentCoroutine;

	public void Comment(Commentary commentary) {
		if (_lastCommentCoroutine != null) {
			_speakText.text = "";
			StopCoroutine(_lastCommentCoroutine);
		}
		
		_lastCommentCoroutine = CharacterComment(commentary);
		StartCoroutine(_lastCommentCoroutine);
	}

	public void StopComment() {
		if (_lastCommentCoroutine != null) {
			_speakText.text = "";
			StopCoroutine(_lastCommentCoroutine);
		}
	}

	private IEnumerator CharacterComment(Commentary commentary) {
		if (commentary.IsContinuous()) {
			for (int i = 0; i < commentary.GetCommentariesCount(); i++) {
				Commentary.Dialog dialog = commentary.GetDialog(i);
				_speakText.text = dialog.text;
				yield return new WaitForSeconds(dialog.duration);
			}
		} else {
			Commentary.Dialog dialog = commentary.GetNextDialog();
			_speakText.text = dialog.text;
			yield return new WaitForSeconds(dialog.duration);
		}
		_speakText.text = "";
	}
}
