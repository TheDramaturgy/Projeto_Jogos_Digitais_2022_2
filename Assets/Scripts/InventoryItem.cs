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
		_previousControlState = GameController.Instance.CanMove();
		GameController.Instance.DisableCharacterMovement();

		transform.SetParent(transform.parent.parent.parent);
		transform.SetAsLastSibling();
		_image.raycastTarget = false;
	}

	public void OnDrag(PointerEventData eventData) {
		transform.position = Input.mousePosition;
	}

	public void OnEndDrag(PointerEventData eventData) {
		if (_previousControlState) GameController.Instance.EnableCharacterMovement();
		else GameController.Instance.DisableCharacterMovement();

		UpdatePosition();
		CheckDropInteraction();
		_image.raycastTarget = true;
	}

	private void CheckDropInteraction() {
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		var hit = Physics2D.GetRayIntersection(ray);

		if (hit.collider != null) {
			var itemIteractable = hit.transform.GetComponent<ItemInteractable>();
			if (itemIteractable != null) {
				itemIteractable.OnDrop(this.gameObject);
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
