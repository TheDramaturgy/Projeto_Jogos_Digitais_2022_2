using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchRoomTrigger : MonoBehaviour {
	public bool isLeftEndTrigger = false;
	private RoomRealocator _realocator;

	private void Start() {
		_realocator = GetComponentInParent<RoomRealocator>();
	}

	private void OnTriggerEnter(Collider other) {
		if (isLeftEndTrigger) {
			_realocator.MoveCharacterLeft();
			_realocator.RotateRight();
		} else {
			_realocator.MoveCharacterRight();
			_realocator.RotateLeft();
		}
	}

	private void OnTriggerExit(Collider other) {
		if (isLeftEndTrigger) {
			_realocator.MoveLeft();
		} else {
			_realocator.MoveRight();
		}
	}
}
