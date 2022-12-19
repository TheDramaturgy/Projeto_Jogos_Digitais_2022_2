using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MinigameTrigger : MonoBehaviour {

	[SerializeField] private UnityEvent _minigameTriggerEvent;
	[SerializeField] private string _minigameSceneName;
	[SerializeField] private GameObjectVariable _clickedItem;

	// ------ Unity Handlers ------

	private void Awake() {
	}

	private void OnMouseUp() {
		_clickedItem.Value = this.gameObject;
		_minigameTriggerEvent.Invoke();
	}

	private void OnTriggerEnter(Collider other) {
		LoadMinigameScene();
	}

	// ------ Methods ------

	public void LoadMinigameScene() {
		SceneManager.LoadScene(_minigameSceneName, LoadSceneMode.Single);
	}
}
