using UnityEngine;

[CreateAssetMenu]
public class BoolVariable : ScriptableObject {
	[SerializeField] private bool _value = false;

	public bool Value { get => _value; set => _value = value; }
}
