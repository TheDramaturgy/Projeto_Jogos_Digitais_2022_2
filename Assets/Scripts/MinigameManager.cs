using UnityEngine;
using UnityEngine.Events;

public class MinigameManager : MonoBehaviour {

	[SerializeField] private RuntimeSet<string> _completeMinigames;
	[SerializeField] private UnityEvent _onSandwichComplete;
	[SerializeField] private UnityEvent _onSandwichIncomplete;

	public static MinigameManager Instance { get; private set; }

	private void Awake() {
		if (Instance != null && Instance != this) Destroy(this);
		else Instance = this;
	}

	public void InvokeAllMinigamesCheck() {
		if (_completeMinigames.Items.Contains("Sandwich")) {
			_onSandwichComplete.Invoke();
		} else {
			_onSandwichIncomplete.Invoke();
		}
	}

}
