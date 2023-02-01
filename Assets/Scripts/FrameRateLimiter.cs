using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FrameRateLimiter : MonoBehaviour {
	[SerializeField] int _frameRate = 60;
	void Start() {
		Application.targetFrameRate = _frameRate;
	}
}
