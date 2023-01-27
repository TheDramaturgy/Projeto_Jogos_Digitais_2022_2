using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collector : MonoBehaviour {
	[SerializeField] private UnityEvent _onCollectionComplete;
	[SerializeField] private List<bool> _collectionItems = new();

	public void SetItemCollected(int index) {
		_collectionItems[index] = true;
		CheckCollectionCompletion();
	}

	public void CollectAnyItem() {
		for (int i = 0; i < _collectionItems.Count; i++) {
			if (!_collectionItems[i]) {
				_collectionItems[i] = true;
				CheckCollectionCompletion();
				return;
			}
		}
	}

	private void CheckCollectionCompletion() {
		for (int i = 0; i < _collectionItems.Count; i++) {
			if (!_collectionItems[i]) { return; }
		}
		_onCollectionComplete.Invoke();
	}
}
