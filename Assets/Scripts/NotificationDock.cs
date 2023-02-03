using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NotificationDock : MonoBehaviour {
	[SerializeField] private float _spacing = 10.0f;

	private List<GameObject> _notifications = new List<GameObject>();
	private float _nextYPosition;
	private int _nextIndex;

	private void Start() {
		_nextYPosition = - _spacing;
		_nextIndex = 0;
	}

	public void ShowNotification(GameObject prefab) {
		if (prefab != null) {
			var rectTransform = prefab.transform.GetComponent<RectTransform>();
			var position = new Vector3(0 - rectTransform.rect.width * 0.5f, 
									   _nextYPosition - rectTransform.rect.height * 0.5f,
									   0.0f);

			var newNotification = Instantiate(prefab, this.transform);
			newNotification.transform.localPosition = position;
			newNotification.GetComponent<Notification>().index = _nextIndex++;
			_notifications.Append(newNotification);

			_nextYPosition = _nextYPosition - rectTransform.rect.height - _spacing;
		}
	}

	public void DeleteNotification(int index) {
		var deletingNotification = _notifications[index];
		_notifications.RemoveAt(index);
		Destroy(deletingNotification);
		UpdateNotificationPositions();
	}

	private void UpdateNotificationPositions() {
		_nextYPosition = -_spacing;
		_nextIndex = 0;
		foreach (var notification in _notifications) {
			var rectTransform = notification.transform.GetComponent<RectTransform>();
			var pos = _nextYPosition - rectTransform.rect.height * 0.5f;
			notification.GetComponent<Notification>().UpdateYPosition(pos);
			notification.GetComponent<Notification>().index = _nextIndex++;
			_nextYPosition = _nextYPosition - rectTransform.rect.height - _spacing;
		}
	}
}
