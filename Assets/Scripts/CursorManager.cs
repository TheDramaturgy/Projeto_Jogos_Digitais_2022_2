using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour {
	[SerializeField] private Texture2D _plainCursor;
	[SerializeField] private Texture2D _interactionCursor;
	private bool _isPlainCursorActive;

	public static CursorManager Instance { get; private set; }

	private void Awake() {
		if (Instance != null && Instance != this) Destroy(this);
		else Instance = this;
	}

	private void Start() {
		_isPlainCursorActive = false;
		ActivePlainCursor();
	}

	public void ActivePlainCursor() {
		if (_isPlainCursorActive) return;
		var hotspot = new Vector2(_plainCursor.width/2, _plainCursor.height/2);
		Cursor.SetCursor(_plainCursor, hotspot, CursorMode.Auto);
		_isPlainCursorActive = true;
	}

	public void ActiveInteractionCursor() {
		if (!_isPlainCursorActive) return;
		var hotspot = new Vector2(_interactionCursor.width / 2, _interactionCursor.height / 2);
		Cursor.SetCursor(_interactionCursor, hotspot, CursorMode.Auto);
		_isPlainCursorActive = false;
	}
}
