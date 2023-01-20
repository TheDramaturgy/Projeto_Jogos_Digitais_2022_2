using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShapeShifter : MonoBehaviour {
	[SerializeField] private List<GameObject> _shapes = new List<GameObject>();
	[SerializeField] private int _defaultShape = 0;
	private int _currentShape = 0;

	private void Start() {
		_currentShape = _defaultShape;
		for (int i = 0; i < _shapes.Count; i++) {
			_shapes[i].SetActive(false);
		}
		_shapes[_currentShape].SetActive(true);
	}

	public void ShapeShift(int index) {
		if (index >= _shapes.Count || index == _currentShape) { return; }
		_shapes[index].SetActive(true);
		_shapes[_currentShape].SetActive(false);
		_currentShape = index;
	}

	public void ShapeCircle() {
		var nextShape = _currentShape + 1;
		if (nextShape >= _shapes.Count) { nextShape = 0; }
		ShapeShift(nextShape);
	}
}
