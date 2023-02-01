using UnityEngine;

public class GameController : MonoBehaviour {
	public static GameController Instance { get; private set; }
	public bool _canInteract = false;

	private void Awake() {
		if (Instance != null && Instance != this) { Destroy(this); }
		else { Instance = this; }
	}

	public void DisableInteraction() { _canInteract = false; }
	public void EnableInteraction() { _canInteract = true; }
	public bool CanInteract() { return _canInteract; }

}
