using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Notification : MonoBehaviour, IPointerDownHandler {
	[SerializeField] private float _activationDelay = 1.0f;

	private bool _isActivating = false;
	private Coroutine _currentAnimation;

	private void Start() {
		_currentAnimation = StartCoroutine(ActivationAnimation());
	}

	private void OnDestroy() {
		DOTween.Kill(transform);
	}

	public void OnPointerDown(PointerEventData eventData) {
		if (_isActivating) {
			if (_currentAnimation != null) StopCoroutine(_currentAnimation);
			_isActivating = false;
		}

		float exitPosition = transform.position.x - GetComponent<RectTransform>().rect.width * 2;
		transform.DOLocalMoveX(exitPosition, 1.0f).SetEase(Ease.InOutBack).OnComplete(() => {
			transform.parent.GetComponent<NotificationDock>().DeleteNotification(gameObject);
		});
	}

	private IEnumerator ActivationAnimation() {
		_isActivating = true;
		yield return new WaitForSeconds(_activationDelay);
		var xPosition = transform.position.x + GetComponent<RectTransform>().rect.width;
		transform.DOLocalMoveX(xPosition, 1.0f).SetEase(Ease.InOutBack).OnComplete(() => {
			_isActivating = false;
		});
	}

	public void UpdateYPosition(float pos) {
		_isActivating = true;
		_currentAnimation = StartCoroutine(UpdateYPositionCoroutine(pos));
	}

	public IEnumerator UpdateYPositionCoroutine(float pos) {
		transform.DOLocalMoveY(pos, 0.5f).SetEase(Ease.InOutQuart).OnComplete(() => {
			_isActivating = false;
		});
		yield break;
	}
}
