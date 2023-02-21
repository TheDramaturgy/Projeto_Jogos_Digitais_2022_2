using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RecipeMinigameValidator : MonoBehaviour {
	[SerializeField] private UnityEvent _onMinigameComplete;
	[SerializeField] private BoolVariable _isRecipeMinigameComplete;

	private List<int> _recipesSlots = new List<int>() { -1, -1, -1, -1, -1, -1, -1, -1 };
	private readonly List<int> RIGHT_RECIPE_ORDER = new List<int> { 20, 21, 22, 23, 24, 25, 26, 27 };

	// ------ Methods ------

	public void UpdateRecipeTextOrder(int slotIdx, int textId) {
		_recipesSlots[slotIdx] = textId;
		CheckRecipeTextOrder();
	}

	private void CheckRecipeTextOrder() {
		if (Compare(_recipesSlots, RIGHT_RECIPE_ORDER)) {
			_isRecipeMinigameComplete.Value = true;
			_onMinigameComplete.Invoke();
		}
	}

	private bool Compare(List<int> a, List<int> b) {
		for (int i = 0; i < a.Count; i++) {
			if (a[i] != b[i]) return false;
		}
		return true;
	}
}
