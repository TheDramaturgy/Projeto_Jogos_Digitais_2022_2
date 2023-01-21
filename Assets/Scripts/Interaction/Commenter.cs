using System.Collections;
using UnityEngine;
using TMPro;

public class Commenter : MonoBehaviour {
	[SerializeField] private TMP_Text _speakText;
	private IEnumerator _lastCommentCoroutine;

	public void Comment(Commentary commentary) {
		if (_lastCommentCoroutine != null) {
			StopCoroutine(_lastCommentCoroutine);
		}
		
		_lastCommentCoroutine = CharacterComment(commentary);
		StartCoroutine(_lastCommentCoroutine);
	}

	private IEnumerator CharacterComment(Commentary commentary) {
		for (int i = 0; i < commentary.GetCommentariesCount(); i++) {
			_speakText.text = commentary.GetCommentary(i);
			yield return new WaitForSeconds(commentary.GetDuration(i));
		}
		_speakText.text = "";
	}
}
