using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {
	private string _sceneName;

	public void SetActiveScene(string sceneName) {
		StartCoroutine(ActivateScene(sceneName));
	}

	public void QueueSetActiveScene(string sceneName) {
		_sceneName = sceneName;
		ActionQueue.Instance.AddAction(TriggerSetActiveScene);
	}

	public void TriggerSetActiveScene() {
		SetActiveScene(_sceneName);
		ActionQueue.Instance.NextAction();
	}

	public void Quit() {
#if UNITY_STANDALONE
		Application.Quit();
#endif
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
	}

	private IEnumerator ActivateScene(string sceneName) {
		yield return null;
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
	}
}
