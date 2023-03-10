using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	[SerializeField] private uint _id;
	private Transform _actualParent;
	private Transform _previousParent;
	private Image _image;
	private bool _previousControlState;

	// ------ Unity Handlers ------
	void Start() {
		_actualParent = transform.parent;
		_image = this.GetComponent<Image>();
	}

	public void OnBeginDrag(PointerEventData eventData) {
		if (_actualParent.GetComponent<InventorySlot>().IsLocked()) return;

		GameController.Instance.SetMovingInventoryItem(true);

		transform.SetParent(transform.parent.parent.parent);
		transform.SetAsLastSibling();
		_image.raycastTarget = false;
	}

	public void OnDrag(PointerEventData eventData) {
		if (_actualParent.GetComponent<InventorySlot>().IsLocked()) return;

		transform.position = Input.mousePosition;
	}

	public void OnEndDrag(PointerEventData eventData) {
		if (_actualParent.GetComponent<InventorySlot>().IsLocked()) return;

		GameController.Instance.SetMovingInventoryItem(false);

		UpdatePosition();
		CheckDropInteraction();
		_image.raycastTarget = true;
	}

	private void CheckDropInteraction() {
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		var hit = Physics2D.GetRayIntersection(ray);

		if (hit.collider != null) {
			var itemIteractable = hit.transform.GetComponent<ItemInteractable>();
			var multipleItemInteractable = hit.transform.GetComponent<MultipleItemInteractable>();
			if (itemIteractable != null) {
				itemIteractable.OnDrop(this.gameObject);
			}
			if (multipleItemInteractable != null) {
				multipleItemInteractable.OnDrop(this.gameObject);
			}
		}
		
	}

	public void ChangeParent(Transform parent) {
		_previousParent = _actualParent;
		_actualParent = parent;
	}

	public void UpdatePosition() {
		transform.SetParent(_actualParent);
		transform.localPosition = Vector3.zero;
	}

	public Transform GetCurrentParent() {
		return _actualParent;
	}

	public Transform GetPreviousParent() {
		if (_previousParent == null) {
			Debug.LogError("No previous parent");
		}
		return _previousParent;
	}

	public void SetId(uint value) {
		_id = value;
	}

	public uint GetId() {
		return _id;
	}

	public InventoryController GetInventoryController() {
		return _actualParent.parent.GetComponent<InventoryController>();
	}
}
