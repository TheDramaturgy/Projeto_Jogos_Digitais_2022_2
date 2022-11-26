using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameObjectUnityEvent : UnityEvent<GameObject> {}

[CreateAssetMenu]
public class GameEvent : ScriptableObject {
	private readonly List<GameEventListener> gameEventListeners = new List<GameEventListener>();

	public void Raise() {
		for (int i = gameEventListeners.Count - 1; i >= 0; i--) {
			GameEventListener listener = gameEventListeners[i];
			listener.OnEventRaised();
		}
	}

	public void RegisterListener(GameEventListener listener) {
		if (!gameEventListeners.Contains(listener)) gameEventListeners.Add(listener);
	}

	public void UnregisterListener(GameEventListener listener) {
		if (gameEventListeners.Contains(listener)) gameEventListeners.Remove(listener);
	}
}
