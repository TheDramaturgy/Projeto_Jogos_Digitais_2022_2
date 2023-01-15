using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class RoomRealocator : MonoBehaviour {
	[SerializeField] private List<Transform> _roomSlices = new List<Transform>();
	[SerializeField] private Transform _character;
	[SerializeField] private float _realocationSize;
	[SerializeField] private NavMeshSurface surface;

	public void RotateRight() {
		var lastRoom = _roomSlices[_roomSlices.Count - 1];
		for (int i = _roomSlices.Count - 1; i > 0; i--) {
			_roomSlices[i] = _roomSlices[i - 1];
		}
		_roomSlices[0] = lastRoom;

		var prevPos = lastRoom.transform.localPosition;
		lastRoom.localPosition = new Vector3(prevPos.x - _roomSlices.Count * _realocationSize, prevPos.y, prevPos.z);

		surface.BuildNavMesh();
	}
	public void RotateLeft() {
		var firstRoom = _roomSlices[0];
		for (int i = 0; i < _roomSlices.Count - 1; i++) {
			_roomSlices[i] = _roomSlices[i + 1];
		}
		_roomSlices[_roomSlices.Count - 1] = firstRoom;

		var prevPos = firstRoom.transform.localPosition;
		firstRoom.localPosition = new Vector3(prevPos.x + _roomSlices.Count * _realocationSize, prevPos.y, prevPos.z);

		surface.BuildNavMesh();
	}

	public void MoveLeft() {
		var prevPos = transform.localPosition;
		transform.localPosition = new Vector3(prevPos.x - _realocationSize, prevPos.y, prevPos.z);
	}

	public void MoveRight() {
		var prevPos = transform.localPosition;
		transform.localPosition = new Vector3(prevPos.x + _realocationSize, prevPos.y, prevPos.z);
	}

	public void MoveCharacterLeft() {
		_character.SetParent(_roomSlices[0].transform, true);
	}

	public void MoveCharacterRight() {
		_character.SetParent(_roomSlices[_roomSlices.Count-1].transform, true);
	}

	
}
