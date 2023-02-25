using UnityEngine;
using UnityEngine.Events;

public class MinigameManager : MonoBehaviour {

	[Header("Sandwich")]
	[SerializeField] private BoolVariable _mgSandwichComplete;
	[SerializeField] private UnityEvent _onSandwichComplete;
	[SerializeField] private UnityEvent _onSandwichIncomplete;

	[Header("Recipe")]
	[SerializeField] private BoolVariable _mgRecipeComplete;
	[SerializeField] private UnityEvent _onRecipeComplete;
	[SerializeField] private UnityEvent _onRecipeIncomplete;

	public static MinigameManager Instance { get; private set; }

	private void Awake() {
		if (Instance != null && Instance != this) Destroy(this);
		else Instance = this;
	}

	public void InvokeAllMinigamesCheck() {
		if (_mgSandwichComplete.Value) {
			_onSandwichComplete.Invoke();
		} else {
			_onSandwichIncomplete.Invoke();
		}

		if (_mgRecipeComplete.Value) {
			_onRecipeComplete.Invoke();
		} else {
			_onRecipeIncomplete.Invoke();
		}
	}

}
