using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {
	public void SetActiveScene(string sceneName) {
		StartCoroutine(ActivateScene(sceneName));
	}

	private IEnumerator ActivateScene(string sceneName) {
		yield return null;
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
	}
}
