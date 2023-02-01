using UnityEngine;

public class FloatAnimation : MonoBehaviour {

	public float degreesPerSecond = 15.0f;
	public float amplitude = 0.5f;
	public float frequency = 1.0f;

	private Vector3 _positionOffset = new Vector3();
	private Vector3 _positionTemporary = new Vector3();

	// ------ Unity Handlers ------

	private void Start() {
		_positionOffset = transform.position;
	}

	private void Update() {
		transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);
		_positionTemporary = _positionOffset;
		_positionTemporary.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude + amplitude;

		transform.position = _positionTemporary;
	}

}
