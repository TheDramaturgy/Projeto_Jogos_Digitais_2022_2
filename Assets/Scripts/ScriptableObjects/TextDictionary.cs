using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TextDictionary : ScriptableObject {
	public Dictionary<uint, string> textDict = new();
}
