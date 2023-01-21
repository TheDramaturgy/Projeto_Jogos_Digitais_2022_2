using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	[SerializeField] private uint _id;
	private Transform _actualParent;
	private Transform _previousParent;
	private Image _image;

	// ------ Unity Handlers ------
	void Start() {
		_actualParent = transform.parent;
		_image = this.GetComponent<Image>();
	}

	public void OnBeginDrag(PointerEventData eventData) {
		transform.SetParent(transform.parent.parent.parent);
		transform.SetAsLastSibling();
		_image.raycastTarget = false;
	}

	public void OnDrag(PointerEventData eventData) {
		transform.position = Input.mousePosition;
	}

	public void OnEndDrag(PointerEventData eventData) {
		UpdatePosition();
	}

	public void ChangeParent(Transform parent) {
		_previousParent = _actualParent;
		_actualParent = parent;
	}

	public void UpdatePosition() {
		transform.SetParent(_actualParent);
		transform.localPosition = Vector3.zero;
		_image.raycastTarget = true;
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
}
