using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandwichMinigameTable : MonoBehaviour {
	[SerializeField] BoolVariable _isOnionCollected;
	[SerializeField] BoolVariable _isCheeseCollected;
	[SerializeField] BoolVariable _isParsleyCollected;

	[SerializeField] GameObject _onion;
	[SerializeField] GameObject _cheese;
	[SerializeField] GameObject _parsley;

	public void CheckItemCollection() {
		if (!_onion.GetComponent<PickupItem>().IsPickedUp) _onion.SetActive(_isOnionCollected.Value);
		if (!_cheese.GetComponent<PickupItem>().IsPickedUp) _cheese.SetActive(_isCheeseCollected.Value);
		if (!_parsley.GetComponent<PickupItem>().IsPickedUp) _parsley.SetActive(_isParsleyCollected.Value);
	}
}
