using UnityEngine;

public class Outline : MonoBehaviour {

	[Header("Base")]
	[SerializeField] private bool _isEnabled = true;
	[SerializeField] private float _outlineSize;
	[SerializeField][ColorUsage(false, true)] private Color _color;
	[SerializeField][Range(0.0f, 1.0f)] private float _minTransparency;
	[SerializeField][Range(0.0f, 1.0f)] private float _maxTransparency;
	[SerializeField][Range(0.0f, 1.0f)] private float _transparencyVariationIntensity;
	[Space(10)]

	[Header("Mouse Over")]
	[SerializeField] private bool _isMouseOverEnabled = true;
	[SerializeField][ColorUsage(false, true)] private Color _mouseOverColor;
	[SerializeField][Range(0.0f, 1.0f)] private float _mouseOverTransparency;

	private Renderer _renderer;
	private float _currentTransparency;
	private bool _isMouseOver = false;
	private bool _isIncreasing;
	private float _effectiveMinTransparency;
	private float _effectiveMaxTransparency;


	void Start() {
		if (!_isEnabled) {
			_effectiveMaxTransparency = 0.0f;
			_effectiveMinTransparency = 0.0f;
		} else {
			_effectiveMaxTransparency = _maxTransparency;
			_effectiveMinTransparency = _minTransparency;
		}

		_renderer = gameObject.GetComponent<Renderer>();
		_renderer.material.SetFloat("_Transparency", _effectiveMinTransparency);
		_renderer.material.SetColor("_Color", _color);
		_renderer.material.SetFloat("_Thickness", _outlineSize);
		_currentTransparency = _effectiveMinTransparency;
		_isIncreasing = true;
	}

	private void Update() {
		if (!_isMouseOver && _effectiveMaxTransparency != _effectiveMinTransparency) {
			if (_currentTransparency >= _effectiveMaxTransparency) {
				_isIncreasing = false;
			} else if (_currentTransparency <= _effectiveMinTransparency) {
				_isIncreasing = true;
			}

			if (_isIncreasing) {
				_currentTransparency += _transparencyVariationIntensity * Time.deltaTime;
			} else {
				_currentTransparency -= _transparencyVariationIntensity * Time.deltaTime;
			}

			_renderer.material.SetFloat("_Transparency", _currentTransparency);
		}
	}

	private void OnMouseEnter() {
		if (_renderer != null && _isMouseOverEnabled) {
			_isMouseOver = true;
			_renderer.material.SetFloat("_Transparency", _mouseOverTransparency);
			_renderer.material.SetColor("_Color", _mouseOverColor);
		}
	}

	private void OnMouseExit() {
		if (_renderer != null && _isMouseOverEnabled) {
			_isMouseOver = false;
			_isIncreasing = true;
			_renderer.material.SetFloat("_Transparency", _effectiveMinTransparency);
			_renderer.material.SetColor("_Color", _color);
		}
	}

	public void SetThickness(float value) {
		_outlineSize = value;
		_renderer.material.SetFloat("_Thickness", _outlineSize);
	}

	public void SetMaxTransparency(float value) {
		_maxTransparency = value;
		_renderer.material.SetFloat("_Transparency", _maxTransparency);
	}

	public void SetMouseOverEnable(bool value) {
		_isMouseOverEnabled = value;
	}

	public void SetEnable(bool value) {
		_isEnabled = value;
		if (_isEnabled) {
			_effectiveMaxTransparency = _maxTransparency;
			_effectiveMinTransparency = _minTransparency;
		} else {
			_effectiveMaxTransparency = 0.0f;
			_effectiveMinTransparency = 0.0f;
		}
	}
}
