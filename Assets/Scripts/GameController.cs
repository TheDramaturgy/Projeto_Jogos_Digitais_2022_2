using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour {
	[SerializeField] private PlayerController _playerController;

	public static GameController Instance { get; private set; }
	public bool _canInteract = false;

	private void Awake() {
		if (Instance != null && Instance != this) Destroy(this);
		else Instance = this;
	}
	public bool CanInteract() => _canInteract;
	public void DisableInteraction() => _canInteract = false;
	public void EnableInteraction() => _canInteract = true;
	public void DisableInteractionDelayed() => StartCoroutine(DisableInteractionAfterSeconds(0.1f));
	public void EnableInteractionDelayed() => StartCoroutine(EnableInteractionAfterSeconds(0.1f));

	public void SetInteractionDelayed(bool value) {
		if (value) {
			StartCoroutine(EnableInteractionAfterSeconds(0.1f));
		} else {
			StartCoroutine(DisableInteractionAfterSeconds(0.1f));
		}
	}

	private IEnumerator EnableInteractionAfterSeconds(float seconds) {
		yield return new WaitForSeconds(seconds);
		_canInteract = true;
	}

	private IEnumerator DisableInteractionAfterSeconds(float seconds) {
		yield return new WaitForSeconds(seconds);
		_canInteract = false;
	}

	public bool CanMove() => _playerController.CanMove();
	public void DisableCharacterMovement() {
		if (_playerController.isActiveAndEnabled) _playerController.SetControlable(false);
	}
	public void EnableCharacterMovement() {
		if (_playerController.isActiveAndEnabled) _playerController.SetControlable(true);
	}
	public void DisableCharacterMovementDelayed() {
		if (_playerController.isActiveAndEnabled) _playerController.SetControlableDelayed(false);
	}
	public void EnableCharacterMovementDelayed() {
		if (_playerController.isActiveAndEnabled) _playerController.SetControlableDelayed(true);
	}

}
