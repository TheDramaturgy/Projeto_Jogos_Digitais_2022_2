using UnityEngine;

[CreateAssetMenu]
public class StringVariable : ScriptableObject {
	[SerializeField] 
	private string _value = "";

	public string Value { get => _value; set => _value = value; }
}
