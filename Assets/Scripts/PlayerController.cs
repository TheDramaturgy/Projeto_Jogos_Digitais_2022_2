using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour {
    private Camera _camera;
    private NavMeshAgent _agent;
    private Transform _transform;
    private Animator _animator;
	private float _interactionRange;
    private bool _isLookingLeft;
	private bool _isMoving;

	[SerializeField] private bool _isControloble;

    [SerializeField] private GameObjectVariable _clickedItem;
    [SerializeField] private Commentary _itemComments;
    [SerializeField] private StringVariable _moveObjective;
	[SerializeField] private UnityEvent _itemPickupEvent;

	[SerializeField] private UnityEvent _moveTargetSetEvent;
	[SerializeField] private UnityEvent _moveTargetReachEvent;
	[SerializeField] private UnityEvent _moveCancelEvent;
	[SerializeField] private Vector3Variable _moveTarget;

	// ------ Unity Handlers ------
	void Awake() {
        _camera = Camera.main;
        if (_camera == null) {
            Debug.LogError("Main Camera not found by PlayerController.");
        }

        _agent = GetComponent<NavMeshAgent>();
        _transform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();

        _isLookingLeft = false;
        _isMoving = false;
	}

	void Start() {
		_moveTargetSetEvent.AddListener(HandleMovement);
	}

	void Update() {
		ListenMouseEvents();
		UpdateMovementStatus();
        HandleAnimation();

		// Maintain sprite turned to camera
        _transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
    }

    // ------ Auxiliary Methods ------

    private void ListenMouseEvents() {
		// Check if left mouse button has been clicked
		if (Input.GetMouseButtonUp(0)) {
			var ray = _camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit)) {
				if (hit.transform.gameObject.tag == "Walkable") {
					_moveTarget.Value = hit.point;
					_moveObjective.Value = "ChangePosition";
					_moveCancelEvent.Invoke();
					_moveTargetSetEvent.Invoke();
				}
			}
		}
	}

    private void UpdateMovementStatus() {
		// Check if character is moving
		if (_agent.velocity.magnitude != 0 && !_isMoving) {
			_isMoving = true;
		} else if (_agent.velocity.magnitude == 0 && _isMoving) {
			_isMoving = false;
			var target = new Vector3(_moveTarget.Value.x, 0.0f, _moveTarget.Value.z);
			var reach = new Vector3(transform.position.x, 0.0f, transform.position.z);
			if (Vector3.Distance(target, reach) <= _interactionRange) {
				_moveTargetReachEvent.Invoke();
				_moveTargetReachEvent.RemoveAllListeners();
				_agent.isStopped = true;
				_agent.ResetPath();
			}
		}
	}

	public void HandleMovement() {
		if (_isControloble) {
			_agent.destination = _moveTarget.Value;
		}
    }

    private void HandleAnimation() {
        if (_agent.velocity.x < 0 && !_isLookingLeft) {
            _isLookingLeft = true;
            _animator.SetBool("isLookingLeft", true);
		} else if (_agent.velocity.x > 0 && _isLookingLeft) {
            _isLookingLeft = false;
			_animator.SetBool("isLookingLeft", false);
		}

        if (_agent.velocity.magnitude != 0 && !_animator.GetBool("isMoving")) {
            _animator.SetBool("isMoving", true);
		} else if (_agent.velocity.magnitude == 0 && _animator.GetBool("isMoving")) {
			_animator.SetBool("isMoving", false);
		}

	}

	// ------ Event Responses ------
	#region Event Responses

	public void MoveCharacterToClickedItem() {
        _moveTarget.Value = _clickedItem.Value.transform.position;
        _moveObjective.Value = "ItemPickup";
        _moveTargetSetEvent.Invoke();
    }

	public void MoveCharacterToClickedItem(float range, float xOffset, UnityAction callback) {
		var targetPosRight = new Vector3(_clickedItem.Value.transform.position.x + xOffset, transform.position.y, _clickedItem.Value.transform.position.z);
		var targetPosLeft = new Vector3(_clickedItem.Value.transform.position.x - xOffset, transform.position.y, _clickedItem.Value.transform.position.z);
		var characterPos = transform.position;
		
		Vector3 targetPos;
		if (Vector3.Distance(targetPosRight, characterPos) < Vector3.Distance(targetPosLeft, characterPos)) {
			targetPos = targetPosRight;
		} else {
			targetPos = targetPosLeft;
		}

		Debug.Log("Target: " + targetPos);
		Debug.Log("Charcter: " + characterPos);
		Debug.Log("Distance: " + Vector3.Distance(targetPos, characterPos));

		
		if (Vector3.Distance(targetPos, characterPos) > range) {
			_interactionRange = range;
			_moveTargetReachEvent.AddListener(callback);
			_moveObjective.Value = "ItemPickup";
			_moveTarget.Value = targetPos;
			_moveTargetSetEvent.Invoke();
		} else {
			callback.Invoke();
		}
	}

	public void PickupClickedItem() {
		if (_moveObjective.Value == "ItemPickup") {
			_itemPickupEvent.Invoke();
		}
	}
	public void SetControlable(bool state) {
		if (state == true) {
			StartCoroutine(EnableControl());
		} else {
			StartCoroutine(DisableControl());
		}
	}

	private IEnumerator EnableControl() {
		yield return new WaitForSeconds(0.6f);
		_isControloble = true;
	}

	private IEnumerator DisableControl() {
		yield return new WaitForSeconds(0.6f);
		_isControloble = false;
	}

	#endregion Event Responses

}
