using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SandwichMinigameValidator : MonoBehaviour {

	[SerializeField] private UnityEvent _onMinigameComplete;
	[SerializeField] private RuntimeSet<string> _completeMinigames;
	[SerializeField] private InventorySet _sandwichInventory;

	// ------ Methods ------

	public void CheckSandwichOrder() {
		if (_sandwichInventory == null || _sandwichInventory.GetSize() != 6) return;
		if (!_sandwichInventory.isOccupied(0) || _sandwichInventory.GetItemFromSlot(0).GetComponent<InventoryItem>().GetId() != 0) return;
		if (!_sandwichInventory.isOccupied(1) || _sandwichInventory.GetItemFromSlot(1).GetComponent<InventoryItem>().GetId() != 3) return;
		if (!_sandwichInventory.isOccupied(2) || _sandwichInventory.GetItemFromSlot(2).GetComponent<InventoryItem>().GetId() != 2) return;
		if (!_sandwichInventory.isOccupied(3) || _sandwichInventory.GetItemFromSlot(3).GetComponent<InventoryItem>().GetId() != 1) return;
		if (!_sandwichInventory.isOccupied(4) || _sandwichInventory.GetItemFromSlot(4).GetComponent<InventoryItem>().GetId() != 4) return;
		if (!_sandwichInventory.isOccupied(5) || _sandwichInventory.GetItemFromSlot(5).GetComponent<InventoryItem>().GetId() != 0) return;

		Debug.Log("COMPLETE!");
		_completeMinigames.Add("Sandwich");
		_onMinigameComplete.Invoke();
	}	
}
