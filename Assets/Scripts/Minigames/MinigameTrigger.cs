using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MinigameTrigger : MonoBehaviour {

	[SerializeField] private string _minigameSceneName;

	// ------ Unity Handlers ------


	// ------ Methods ------

	public void LoadMinigameScene() {
		SceneManager.LoadScene(_minigameSceneName, LoadSceneMode.Single);
	}
}
