using UnityEngine;

public class RecipeSlot : MonoBehaviour {

	[SerializeField] private int _idx;
	[SerializeField] private int _textId = RecipeText.UNASSIGNED_ID;
	[SerializeField] private RecipeMinigameValidator _validator;

	// ------ Unity Handlers ------

	private void Start () {
		SetRecipeText(RecipeText.UNASSIGNED_ID);
	}

	private void OnTriggerStay2D(Collider2D collision) {
		var recipeText = collision.transform.GetComponent<RecipeText>();
		var distanceFromThis = Vector2.Distance(collision.transform.position, transform.position);

		if (_textId == RecipeText.UNASSIGNED_ID && recipeText.GetParentIndex() != _idx && distanceFromThis < recipeText.DistanceFromParent()) {
			SetRecipeText(recipeText.AssignSlot(transform, _idx));
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		var recipeText = collision.transform.GetComponent<RecipeText>();
		if (recipeText.GetId() == _textId) SetRecipeText(RecipeText.UNASSIGNED_ID);
		if (recipeText.GetParentIndex() == _idx) recipeText.Unassign();
	}


	private void SetRecipeText(int textIdx) {
		_textId = textIdx;
		_validator.UpdateRecipeTextOrder(_idx, textIdx);
	}


	public void SetIndex(int idx) {
		_idx = idx;
	}

	public int GetIndex() {
		return _idx;
	}
}
