using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class InventorySet : ScriptableObject {
	struct Slot {
		public bool IsOcupied;
		public GameObject Item;

		public Slot(bool isOcupied) {
			this.IsOcupied = isOcupied;
			this.Item = null;
		}

		public Slot(GameObject item) {
			this.IsOcupied = true;
			this.Item = item;
		}
	}

	private List<Slot> _slots;

	public bool GetFreeSlot(out int idx) {
		if (_slots == null) {
			_slots = new List<Slot>();
			for (int i = 0; i < 6; ++i) _slots.Add(new Slot(false));

			idx = 0;
			return true;
		}

		for (int i = 0; i < 6; ++i) {
			if (!_slots[i].IsOcupied) {
				idx = i;
				return true;
			}
		}

		idx = -1;
		return false;
	}

	public bool GetFreeSlot(uint quantity, out List<int> idx) {
		idx = new List<int>();

		// If slots not intantiated, intantiate.
		if (_slots == null) {
			_slots = new List<Slot>();
			for (int i = 0; i < 6; ++i) _slots.Add(new Slot(false));
		}

		// Find free slots.
		for (int i = 0; i < 6; ++i) {
			if (!_slots[i].IsOcupied) { idx.Add(i); }
			if (idx.Count == quantity) { return true; }
		}

		// Not enough free slots.
		idx = null;
		return false;
	}

	public void AddItemToSlot(GameObject item, int slot) {
		_slots[slot] = new Slot(item);
	}

	public GameObject GetItemFromSlot(int slot) {
		return _slots[slot].Item;
	}

	public void RemoveItem(GameObject item) {
		for (int i = 0; i < _slots.Count; ++i) {
			if (_slots[i].Item == item) {
				_slots[i] = new Slot(false);
			}
		}
	}

	public void SwapItemsBetweenSlots(int slot1, int slot2) {
		var item1 = _slots[slot1].Item;
		var item2 = _slots[slot2].Item;

		RemoveItem(item1);
		RemoveItem(item2);

		AddItemToSlot(item1, slot2);
		AddItemToSlot(item2, slot1);
	}

	public bool isOccupied(int idx) {
		return _slots[idx].IsOcupied;
	}

	public int GetSize() {
		if (_slots == null) {
			return 0;
		} else {
			return _slots.Count;
		}
	}
}
