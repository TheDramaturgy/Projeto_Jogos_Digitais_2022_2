using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour {
	[SerializeField] List<string> _scenes = new List<string>();
	[SerializeField] int _mainSceneIndex = 0;
	[SerializeField] private RuntimeSet<string> _completeMinigames;

	private void Start() {
		_completeMinigames.Items.Clear();
		foreach (var name in _scenes) {
			StartCoroutine(LoadScene(name));
		}
		MinigameManager.Instance.InvokeAllMinigamesCheck();
	}

	private IEnumerator LoadScene(string name) {
		if (SceneManager.GetSceneByName(name).isLoaded) {
			yield break;
		}

		AsyncOperation operation = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);

		while (!operation.isDone) yield return null;

		if (_scenes[_mainSceneIndex].Equals(name)) {
			SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
		}
	}

}
