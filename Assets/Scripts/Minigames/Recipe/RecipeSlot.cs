using UnityEngine;

public class RecipeSlot : MonoBehaviour {

	[SerializeField] private int _slotIndex;
	[SerializeField] private int _occupyingIndex = RecipeText.UNASSIGNED_ID;

	// ------ Unity Handlers ------

	private void Start () {
		_occupyingIndex = RecipeText.UNASSIGNED_ID;
	}

	private void OnTriggerStay2D(Collider2D collision) {
		var recipeText = collision.transform.GetComponent<RecipeText>();
		var distanceFromThis = Vector2.Distance(collision.transform.position, transform.position);

		if (_occupyingIndex == RecipeText.UNASSIGNED_ID && recipeText.GetParentIndex() != _slotIndex && distanceFromThis < recipeText.DistanceFromParent()) {
			_occupyingIndex = recipeText.AssignSlot(transform, _slotIndex);
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		var recipeText = collision.transform.GetComponent<RecipeText>();
		if (recipeText.GetId() == _occupyingIndex) _occupyingIndex = RecipeText.UNASSIGNED_ID;
		if (recipeText.GetParentIndex() == _slotIndex) recipeText.Unassign();
	}


	public void SetIndex(int idx) {
		_slotIndex = idx;
	}

	public int GetIndex() {
		return _slotIndex;
	}
}
