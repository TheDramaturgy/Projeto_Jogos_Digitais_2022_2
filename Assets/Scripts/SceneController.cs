using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
	[SerializeField] private UnityEvent _onSceneActivation;
	[SerializeField] private UnityEvent _onSceneDeactivation;
	[SerializeField] private string _thisSceneName;
	private Scene _thisScene;
	private Coroutine _sceneActivationCoroutine;

	private void Start() {
		_thisScene = SceneManager.GetSceneByName(_thisSceneName);
		SceneManager.activeSceneChanged += ChangedActiveScene;
	}

	private void ChangedActiveScene(Scene current, Scene next) {
		if (next == _thisScene) {
			_sceneActivationCoroutine = StartCoroutine(SceneActivationEvent());
		} else if (current == _thisScene) {
			if (_sceneActivationCoroutine != null) StopCoroutine(_sceneActivationCoroutine);
			_onSceneDeactivation.Invoke();
		}
	}

	private IEnumerator SceneActivationEvent() {
		yield return new WaitForSeconds(0.1f);
		_onSceneActivation.Invoke();
	}
}
