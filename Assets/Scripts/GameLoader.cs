using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour {
	[SerializeField] int _mainSceneIndex = 0;
	[SerializeField] private RuntimeSet<string> _completeMinigames;
	[SerializeField] List<string> _scenes = new List<string>();
	[SerializeField] List<BoolVariable> _boolVariables = new List<BoolVariable>();

	private void Start() {
		_completeMinigames.Items.Clear();
		foreach (var name in _scenes) {
			StartCoroutine(LoadScene(name));
		}
		ResetBoolVariables();
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

	private void ResetBoolVariables() {
		foreach (var boolVar in _boolVariables) {
			boolVar.Value = false;
		}
	}

	private void UnloadAllScenes() {
		foreach (var sceneName in _scenes) {
			var operation = SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneName));
			while(!operation.isDone) { continue; }
		}
	}

}
