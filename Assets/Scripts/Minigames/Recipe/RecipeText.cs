using System;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class RecipeText : MonoBehaviour {
	[SerializeField] private int _id;

	private Transform _unassigned;
	private Transform _actualParent;
	private int _actualParentIndex;
	private Rigidbody2D _rigidbody;
	private Collider2D _collider;
	private bool _isLocked;
	[SerializeField] private bool _isAssigned;
	[SerializeField] private bool _isBeingDragged;

	[HideInInspector] public static int UNASSIGNED_ID = -1;

	// ------ Unity Handlers ------
	void Start() {
		_rigidbody = GetComponent<Rigidbody2D>();
		_collider = GetComponent<Collider2D>();
		_actualParent = transform.parent;
		_unassigned = transform.parent;
		_actualParentIndex = -1;
		_isAssigned = false;
		_isLocked = false;
	}

	private void OnMouseDown() {
		if (_isLocked) return;

		transform.SetParent(transform.parent.parent);
		transform.SetAsLastSibling();
		transform.rotation = Quaternion.identity;
		_rigidbody.bodyType = RigidbodyType2D.Kinematic;
		_collider.isTrigger = false;
		_isBeingDragged = true;
	}

	private void OnMouseDrag() {
		if (_isLocked) return;

		float zPos = Math.Abs(Camera.main.transform.position.z) - Math.Abs(transform.position.z);
		var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zPos);
		var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
		transform.position = new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
		transform.rotation = Quaternion.identity;
	}


	private void OnMouseUp() {
		if (_isLocked) return;

		var recipeSlot = _actualParent.gameObject.GetComponent<RecipeSlot>();

		transform.SetParent(_actualParent);
		if (_isAssigned) {
			_rigidbody.velocity = Vector3.zero;
			_rigidbody.angularVelocity = 0.0f;
			_collider.isTrigger = true;
			transform.localPosition = Vector3.zero;
			transform.rotation = Quaternion.identity;
		} else {
			_rigidbody.bodyType = RigidbodyType2D.Dynamic;
		}
		_isBeingDragged = false;

		if (recipeSlot != null) {
			recipeSlot.ChildRecipeTextDragEnded();
		}
	}


	public int AssignSlot(Transform slot, int index) {
		if (_isBeingDragged) {
			_actualParent = slot;
			_actualParentIndex = index;
			_isAssigned = true;
			return _id;
		}
		return UNASSIGNED_ID;
	}

	public int Unassign() {
		_actualParent = _unassigned;
		_actualParentIndex = -1;
		_isAssigned = false;
		return UNASSIGNED_ID;
	}

	public float DistanceFromParent() {
		if (_isAssigned) return Vector2.Distance(transform.position, _actualParent.position);
		return float.PositiveInfinity;
	}

	public void Lock() => _isLocked = true;

	public int GetParentIndex() => _actualParentIndex;

	public void SetId(int value) {
		_id = value;
	}

	public int GetId() {
		return _id;
	}
}
