using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameObjectVariable : ScriptableObject {
	
	#if UNITY_EDITOR
	[Multiline]
	public string Description = "";
	#endif

	[SerializeField]
	private GameObject _gameObject = null;

	public GameObject Value { get => _gameObject; set => _gameObject = value; }

	public void Clear() {
		_gameObject = null;
	}
}
