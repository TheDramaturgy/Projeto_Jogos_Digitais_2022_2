using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	private Transform _actualParent;
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
		transform.SetParent(_actualParent);
		transform.localPosition = Vector3.zero;
		_image.raycastTarget = true;
	}

	public void ChangeParent(Transform parent) {
		Debug.Log("New Parent: " + parent.gameObject.name);
		_actualParent = parent;
	}
}
