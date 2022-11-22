using System.Collections;
using System.Collections.Generic;
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

	public void AddItemToSlot(GameObject item, int slot) {
		_slots[slot] = new Slot(item);
	}

	public void RemoveItem(GameObject item) {
		for (int i = 0; i < _slots.Count; ++i) {
			if (_slots[i].Item == item) {
				_slots[i] = new Slot(false);
			}
		}
	}

	public bool isOcupied(int idx) {
		return _slots[idx].IsOcupied;
	}
}
