using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MoveTarget : ScriptableObject {
	private Vector3 destination;

	public Vector3 Destination { get => destination; set => destination = value; }
}
