using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
	[SerializeField] private UnityEvent _onSceneActivation;
	[SerializeField] private UnityEvent _onSceneDeactivation;
	[SerializeField] private string _thisSceneName;
	private Scene _thisScene;

	private void Start() {
		_thisScene = SceneManager.GetSceneByName(_thisSceneName);
		SceneManager.activeSceneChanged += ChangedActiveScene;
	}

	private void ChangedActiveScene(Scene current, Scene next) {
		if (next == _thisScene) {
			StartCoroutine(SceneActivationEvent());
		} else if (current == _thisScene) {
			_onSceneDeactivation.Invoke();
		}
	}

	private IEnumerator SceneActivationEvent() {
		yield return new WaitForSeconds(Time.deltaTime * 5);
		_onSceneActivation.Invoke();
	}
}
