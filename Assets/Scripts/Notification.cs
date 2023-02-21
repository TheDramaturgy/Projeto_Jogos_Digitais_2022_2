using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;

public class Notification : MonoBehaviour, IPointerDownHandler {
	[SerializeField] private float _activationDelay = 1.0f;
	[SerializeField] private float _timeUntilExit = -1.0f;

	private bool _isActivating = false;
	private bool _isWaitingToExit = false;
	private Coroutine _currentAnimation;

	[Header("Commentary")]
	[SerializeField] private TMP_Text _speakText;
	[SerializeField] private Commentary _commentary;

	private IEnumerator _lastCommentCoroutine;
	private Commentary.Dialog _currentDialog;
	private int _nextCommentIndex = 0;
	private float _writingSpeed = 0.025f;
	private bool _isCommenting = false;
	private bool _isWriting = false;

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
		} else if (_isCommenting) {
			SkipDialogWriting();
		} else {
			ExitNotification();
		}
	}

	#region Notification Animation

	private IEnumerator ActivationAnimation() {
		_isActivating = true;
		yield return new WaitForSeconds(_activationDelay);
		var xPosition = transform.position.x + GetComponent<RectTransform>().rect.width;
		transform.DOLocalMoveX(xPosition, 1.0f).SetEase(Ease.InOutBack).OnComplete(() => {
			_isActivating = false;
			TriggerComment();
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

	public void ExitNotification() {
		if (_isWaitingToExit) StopCoroutine(_currentAnimation);

		DOTween.Kill(transform);
		float exitPosition = transform.position.x - GetComponent<RectTransform>().rect.width * 2;
		transform.DOLocalMoveX(exitPosition, 1.0f).SetEase(Ease.InOutBack).OnComplete(() => {
			transform.parent.GetComponent<NotificationDock>().DeleteNotification(gameObject);
		});
	}

	public IEnumerator WaitUntilExitTime() {
		if (_timeUntilExit < 0.0f) yield break;

		_isWaitingToExit = true;
		yield return new WaitForSeconds(_timeUntilExit);

		ExitNotification();
	}

	#endregion

	#region Commentary

	public void TriggerComment() {
		if (_commentary == null) {
			_currentAnimation = StartCoroutine(WaitUntilExitTime());
			return;
		}

		_isCommenting = true;

		_nextCommentIndex = 0;
		_currentDialog = _commentary.IsContinuous() ? _commentary.GetDialog(_nextCommentIndex) : _commentary.GetNextDialog();
		_nextCommentIndex++;

		_lastCommentCoroutine = WriteDialogOverTime();
		StartCoroutine(_lastCommentCoroutine);
	}

	private void EndComment() {
		_isCommenting = false;
		_currentAnimation = StartCoroutine(WaitUntilExitTime());
	}

	private IEnumerator WriteDialogOverTime() {
		_isWriting = true;

		var position = 0;
		while (position < _currentDialog.text.Length) {
			do {
				_speakText.text += _currentDialog.text[position++];
				if (position >= _currentDialog.text.Length) break;
			} while (_currentDialog.text[position] == ' ');
			yield return new WaitForSeconds(_writingSpeed);
		}

		_isWriting = false;
		EndComment();
	}

	private void SkipDialogWriting() {
		if (_isWriting) {
			StopCoroutine(_lastCommentCoroutine);
			_speakText.text = _currentDialog.text;
			_isWriting = false;
			EndComment();
		}
	}

	#endregion
}
