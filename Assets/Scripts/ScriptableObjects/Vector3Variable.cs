using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Vector3Variable : ScriptableObject {
	private Vector3 _vector3;

	public Vector3 Value { get => _vector3; set => _vector3 = value; }
}
