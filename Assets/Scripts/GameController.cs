using UnityEngine;

public class GameController : MonoBehaviour {
	[SerializeField] private PlayerController _playerController;

	public static GameController Instance { get; private set; }
	public bool _canInteract = false;

	private void Awake() {
		if (Instance != null && Instance != this) Destroy(this);
		else Instance = this;
	}

	public void DisableInteraction() { _canInteract = false; }
	public void EnableInteraction() { _canInteract = true; }
	public bool CanInteract() { return _canInteract; }

	public void DisableCharacterMovement() {
		_playerController.SetControlable(false);
	}

	public void EnableCharacterMovement() {
		_playerController.SetControlable(true);
	}

}
