using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ClickedItem : ScriptableObject {
	private GameObject itemGameObject = null;

	public GameObject ItemGameObject { get => itemGameObject; set => itemGameObject = value; }

	public void Clear() {
		itemGameObject = null;
	}
}
