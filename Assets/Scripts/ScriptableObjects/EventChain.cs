using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu]
public class EventChain : ScriptableObject {
	[SerializeField] private UnityEvent[] _events;

}
