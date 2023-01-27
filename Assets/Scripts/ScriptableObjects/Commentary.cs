using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Commentary : ScriptableObject {
	[SerializeField] private List<string> Commentaries = new();
	[SerializeField] private List<float>  Durations = new();
	[SerializeField] private bool _isContinuous = false;
	[SerializeField] private bool _isCyclic = false;
	[SerializeField] private int _actualComment = 0;

	public struct Dialog {
		public string text;
		public float duration;

		public Dialog(string text, float duration) {
			this.text = text;
			this.duration = duration;
		}
	}

	private void OnValidate() {
		while (Commentaries.Count > Durations.Count) {
			Durations.Add(5.0f);
		}
	}

	public int GetCommentariesCount() {
		return _isContinuous ? Commentaries.Count : 1;
	}

	public Dialog GetDialog(int index) {
		if (index < Commentaries.Count) { return new Dialog(Commentaries[index], Durations[index]); } 
		else {
			Debug.LogError("Commentary index out of bound.");
			Debug.LogError(this.name + " have " + Commentaries.Count + " Commentaries, and element " + index + " was inquired.");
			return new Dialog();
		}
	}

	public Dialog GetNextDialog() {
		Dialog dialog;

		if (_isCyclic) {
			dialog = new Dialog(Commentaries[_actualComment], Durations[_actualComment]);
			_actualComment = (_actualComment + 1) >= Commentaries.Count ? 0 : _actualComment + 1;
		} else {
			if (_actualComment >= Commentaries.Count) {
				dialog = new Dialog("", 0.1f);
			} else {
				dialog = new Dialog(Commentaries[_actualComment], Durations[_actualComment]);
				_actualComment++;
			}
		}

		return dialog;
	}

	public bool IsContinuous() {
		return _isContinuous;
	}
}
