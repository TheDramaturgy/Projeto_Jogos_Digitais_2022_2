using System.Collections;
using UnityEngine;
using TMPro;

public class Commenter : MonoBehaviour {
	[SerializeField] private TMP_Text _speakText;

	public void Comment(string commentary) {
		_speakText.text = commentary;
		StartCoroutine(ClearDialog(5));
	}

	private IEnumerator ClearDialog(int time) {
		yield return new WaitForSeconds(time);
		_speakText.text = "";
	}
}
