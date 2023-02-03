using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NotificationDock : MonoBehaviour {
	[SerializeField] private float _spacing = 10.0f;

	private List<GameObject> _notifications = new List<GameObject>();
	private float _nextYPosition;
	private Coroutine _lastPositionUpdate;

	private void Start() {
		_nextYPosition = -_spacing;
	}

	public void ShowNotification(GameObject prefab) {
		if (prefab != null) {
			var rectTransform = prefab.transform.GetComponent<RectTransform>();
			var position = new Vector3(0 - rectTransform.rect.width * 0.5f, 
									   _nextYPosition - rectTransform.rect.height * 0.5f,
									   0.0f);

			var newNotification = Instantiate(prefab, this.transform);
			newNotification.transform.localPosition = position;
			_notifications.Add(newNotification);

			_nextYPosition = _nextYPosition - rectTransform.rect.height - _spacing;
		}
	}

	public void DeleteNotification(GameObject notification) {
		if (_lastPositionUpdate != null) StopCoroutine(_lastPositionUpdate);
		_notifications.Remove(notification);
		Destroy(notification);
		_lastPositionUpdate = StartCoroutine(UpdateNotificationPositions());
	}

	private IEnumerator UpdateNotificationPositions() {
		_nextYPosition = -_spacing;
		foreach (var notification in _notifications) {
			var rectTransform = notification.transform.GetComponent<RectTransform>();
			var pos = _nextYPosition - rectTransform.rect.height * 0.5f;
			notification.GetComponent<Notification>().UpdateYPosition(pos);
			_nextYPosition = _nextYPosition - rectTransform.rect.height - _spacing;
		}
		yield break;
	}
}
