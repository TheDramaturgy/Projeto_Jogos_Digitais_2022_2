using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MinigameTrigger : MonoBehaviour {

	[SerializeField] private UnityEvent _minigameTriggerEvent;
	[SerializeField] private string _minigameSceneName;

	// ------ Unity Handlers ------

	private void Awake() {
		_minigameTriggerEvent.AddListener(LoadMinigameScene);
	}

	private void OnMouseUp() {
		_minigameTriggerEvent.Invoke();
	}

	// ------ Methods ------

	public void LoadMinigameScene() {
		SceneManager.LoadScene(_minigameSceneName, LoadSceneMode.Single);
	}
}
