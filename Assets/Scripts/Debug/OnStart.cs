using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnStart : MonoBehaviour {
	[SerializeField] private UnityEvent _onStart;
	void Start() {
		_onStart.Invoke();
	}

}
