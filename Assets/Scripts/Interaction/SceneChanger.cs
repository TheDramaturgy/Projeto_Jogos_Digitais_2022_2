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

	private IEnumerator ActivateScene(string sceneName) {
		yield return null;
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
	}
}
