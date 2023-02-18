using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TeleportBack : MonoBehaviour {
	public Vector3 position;

	private Collider2D _collider;

	private void Start() {
		_collider = GetComponent<Collider2D>();
		_collider.isTrigger = true;
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		var newPosition = new Vector3(position.x, position.y, collision.transform.localPosition.z);
		collision.transform.SetLocalPositionAndRotation(newPosition, Quaternion.identity);
	}

	private void OnTriggerStay2D(Collider2D collision) {
		var newPosition = new Vector3(position.x, position.y, collision.transform.localPosition.z);
		collision.transform.SetLocalPositionAndRotation(newPosition, Quaternion.identity);
	}
}
