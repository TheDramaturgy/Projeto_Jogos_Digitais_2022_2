using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TableSandwich : MonoBehaviour {
	[SerializeField] private RuntimeSet<string> _completeMinigames;
	[SerializeField] private bool _onSandwichMinigameComplete;

	// ------ Unity Handlers ------

	void Awake() {
		SceneManager.sceneLoaded += onSceneLoad;
	}

	private void Start() {
		if (_completeMinigames.Items.Contains("Sandwich")) {
			transform.GetChild(0).gameObject.SetActive(_onSandwichMinigameComplete);
			transform.GetChild(1).gameObject.SetActive(_onSandwichMinigameComplete);
		}
	}

	void onSceneLoad(Scene scene, LoadSceneMode mode) {
		if (_completeMinigames.Items.Contains("Sandwich")) {
			transform.GetChild(0).gameObject.SetActive(_onSandwichMinigameComplete);
			transform.GetChild(1).gameObject.SetActive(_onSandwichMinigameComplete);
		}
	}

	void OnDestroy() {
		SceneManager.sceneLoaded -= onSceneLoad;
	}
}
