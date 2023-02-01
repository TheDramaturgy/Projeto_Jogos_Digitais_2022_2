using System.Collections;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour {
    private Camera _camera;
    private NavMeshAgent _agent;
    private Transform _transform;
    private Animator _animator;
	private float _interactionRange;
    private bool _isLookingLeft;

	[SerializeField] private bool _canMove = false;

    [SerializeField] private GameObjectVariable _clickedItem;
    [SerializeField] private StringVariable _moveObjective;
	[SerializeField] private UnityEvent _itemPickupEvent;

	[SerializeField] private UnityEvent _moveTargetSetEvent;
	[SerializeField] private UnityEvent _moveTargetReachEvent;
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
		if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject() && _canMove) {
			var ray = _camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit)) {
				if (hit.transform.gameObject.tag == "Walkable") {
					_moveTargetReachEvent.RemoveAllListeners();
					_moveTarget.Value = hit.point;
					_moveObjective.Value = "ChangePosition";
					_moveTargetSetEvent.Invoke();
				}
			}
		}
	}

    private void UpdateMovementStatus() {
		var target = new Vector3(_moveTarget.Value.x, transform.position.y, _moveTarget.Value.z);
		if (Vector3.Distance(target, transform.position) <= _interactionRange) {
			_moveTargetReachEvent.Invoke();
			_moveTargetReachEvent.RemoveAllListeners();
			_agent.isStopped = true;
			_agent.ResetPath();
		}
	}

	public void HandleMovement() {
		_agent.destination = _moveTarget.Value;
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

	public void MoveCharacterToClickedItem(float range, float xOffset, UnityAction callback, bool isException = false) {
		if (!_canMove && !isException) { return; }

		var targetPosRight = new Vector3(_clickedItem.Value.transform.position.x + xOffset, transform.position.y, _clickedItem.Value.transform.position.z);
		var targetPosLeft = new Vector3(_clickedItem.Value.transform.position.x - xOffset, transform.position.y, _clickedItem.Value.transform.position.z);
		var characterPos = transform.position;
		
		Vector3 targetPos;
		if (Vector3.Distance(targetPosRight, characterPos) < Vector3.Distance(targetPosLeft, characterPos)) {
			targetPos = targetPosRight;
		} else {
			targetPos = targetPosLeft;
		}

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

	public void SetControlable(bool state) {
		_canMove = state;
	}

	public bool CanMove() { return _canMove; }

	#endregion Event Responses

}
