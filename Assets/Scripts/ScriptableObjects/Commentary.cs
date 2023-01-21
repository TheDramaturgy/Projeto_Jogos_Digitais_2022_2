using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Commentary : ScriptableObject {
	[SerializeField] private List<string> Commentaries = new();
	[SerializeField] private List<float>  Durations = new();

	private void OnValidate() {
		while (Commentaries.Count > Durations.Count) {
			Durations.Add(5.0f);
		}
	}

	public int GetCommentariesCount() {
		return Commentaries.Count;
	}

	public string GetCommentary(int index) {
		if (index < Commentaries.Count) { return Commentaries[index]; } 
		else {
			Debug.LogError("Commentary index out of bound.");
			Debug.LogError(this.name + " have " + Commentaries.Count + " Commentaries, and element " + index + " was inquired.");
			return null;
		}
	}

	public float GetDuration(int index) {
		if (index < Durations.Count) { return Durations[index]; } 
		else {
			Debug.LogError("Duration index out of bound.");
			Debug.LogError(this.name + " have " + Durations.Count + " Commentary durations, and element " + index + " was inquired.");
			return 0.0f;
		}
	}
}
