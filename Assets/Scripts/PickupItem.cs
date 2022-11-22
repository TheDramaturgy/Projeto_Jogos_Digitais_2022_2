using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PickupItem : MonoBehaviour {

	[SerializeField] private ClickedItem clickedItem;
	[SerializeField] private UnityEvent ItemClickEvent;
	public Image ItemPrefab;

	// ------ Unity Handlers ------

	public void OnMouseUp() {
		clickedItem.ItemGameObject = this.gameObject;
		ItemClickEvent.Invoke();
	}
}
