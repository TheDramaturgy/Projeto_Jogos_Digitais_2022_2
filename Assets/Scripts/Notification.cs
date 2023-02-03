using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Notification : MonoBehaviour, IPointerDownHandler {
	[SerializeField] private float _activationDelay = 1.0f;

	private void OnEnable() {
		StartCoroutine(ActivationAnimation());
	}

	private void OnDestroy() {
		DOTween.KillAll();
	}

	public void OnPointerDown(PointerEventData eventData) {
		float exitPosition = this.transform.position.x - this.GetComponent<RectTransform>().rect.width;
		this.transform.DOLocalMoveX(exitPosition, 1.0f).SetEase(Ease.InOutBack).OnComplete(() => {
			transform.parent.GetComponent<NotificationDock>().DeleteNotification(this.gameObject);
		});
	}

	private IEnumerator ActivationAnimation() {
		yield return new WaitForSeconds(_activationDelay);
		float exitPosition = this.transform.position.x + this.GetComponent<RectTransform>().rect.width;
		transform.DOLocalMoveX(exitPosition, 1.0f).SetEase(Ease.InOutBack);
	}

	public void UpdateYPosition(float pos) {
		this.transform.DOLocalMoveY(pos, 0.5f).SetEase(Ease.InOutQuart);
	}
}
