using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationDock : MonoBehaviour {
	[SerializeField] private float _spacing = 10.0f;

	private Queue<GameObject> _notificationsNotInstantiated = new Queue<GameObject>();
	private List<GameObject> _notifications = new List<GameObject>();
	private List<GameObject> _notificationsActionQueue = new List<GameObject>();
	private float _nextYPosition;
	private Coroutine _lastPositionUpdate;
	private GameObject _currentActionQueueNotification;

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


	public void AddNotificationToQueue(GameObject prefab) {
		if (prefab != null) {
			_notificationsNotInstantiated.Enqueue(prefab);
			ActionQueue.Instance.AddAction(TriggerShowNotification, InterruptShowNotification);
		}
	}

	public void InterruptShowNotification() {
		if (_currentActionQueueNotification != null) {
			Debug.Log("Deleting Notification");
			DeleteNotification(_currentActionQueueNotification);
		}
	}

	public void TriggerShowNotification() {
		GameObject prefab = null;
		if (_notificationsNotInstantiated.Count > 0) prefab = _notificationsNotInstantiated.Dequeue();
	
		if (prefab != null) {
			var rectTransform = prefab.transform.GetComponent<RectTransform>();
			var position = new Vector3(0 - rectTransform.rect.width * 0.5f,
									   _nextYPosition - rectTransform.rect.height * 0.5f,
									   0.0f);

			_currentActionQueueNotification = Instantiate(prefab, this.transform);
			_currentActionQueueNotification.transform.localPosition = position;
			_notifications.Add(_currentActionQueueNotification);
			_notificationsActionQueue.Add(_currentActionQueueNotification);

			_nextYPosition = _nextYPosition - rectTransform.rect.height - _spacing;
		} else {
			Debug.Log("Prefab is NULL");
		}
	}

	public void DeleteNotification(GameObject notification) {
		if (_lastPositionUpdate != null) StopCoroutine(_lastPositionUpdate);

		_notifications.Remove(notification);
		if (_notificationsActionQueue.Contains(notification)) {
			_notificationsActionQueue.Remove(notification);
			ActionQueue.Instance.NextAction();
		}

		Destroy(notification);
		_lastPositionUpdate = StartCoroutine(UpdateNotificationPositions());
	}

	public void ClearAllNotifications() {
		ActionQueue.Instance.AddAction(TriggerClearAllNotifications);
	}


	public void TriggerClearAllNotifications() {
		foreach (var notification in _notifications) {
			notification.GetComponent<Notification>().ExitNotification();
		}
		ActionQueue.Instance.NextAction();
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
