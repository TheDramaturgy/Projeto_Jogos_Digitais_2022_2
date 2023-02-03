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
	[SerializeField] private UnityEvent _itemPickupEvent;
	[SerializeField] private UnityEvent _moveTargetSetEvent;
	[SerializeField] private UnityEvent _moveTargetReachEvent;

	[SerializeField] private Vector3Variable _moveTarget;
	[SerializeField] private float _maxZPoint;
	[SerializeField] private float _minZPoint;

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
		_moveTargetSetEvent.AddListener(SetDestination);
	}

	void Update() {
		ListenMouseEvents();
		UpdateMovementStatus();
        HandleAnimation();

		// Fix sprite face to camera
        _transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
    }

    // ------ Auxiliary Methods ------

    private void ListenMouseEvents() {
		// Check if left mouse button is being held down
		if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && _canMove) {
			var ray = _camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit)) {
				if (hit.transform.gameObject.tag == "Walkable") {
					_moveTargetReachEvent.RemoveAllListeners();

					_moveTarget.Value = hit.point;
					_moveTargetSetEvent.Invoke();
				} else if (hit.transform.gameObject.tag != "Interactable") {
					_moveTargetReachEvent.RemoveAllListeners();

					var zPoint = hit.point.z > _maxZPoint ? _maxZPoint : hit.point.z;
					zPoint = zPoint < _minZPoint ? _minZPoint : zPoint;
					_moveTarget.Value = new Vector3(hit.point.x, this.transform.position.y, zPoint);
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

	public void SetDestination() {
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
			_moveTarget.Value = targetPos;
			_moveTargetSetEvent.Invoke();
		} else {
			callback.Invoke();
		}
	}

	public void SetControlable(bool state) {
		_canMove = state;
	}

	public void SetControlableDelayed(bool state) {
		if (state) StartCoroutine(EnableControl());
		else StartCoroutine(DisableControl());
	}

	private IEnumerator EnableControl() {
		yield return new WaitForSeconds(0.1f);
		_canMove = true;
	}

	private IEnumerator DisableControl() {
		yield return new WaitForSeconds(0.1f);
		_canMove = false;
	}

	public bool CanMove() { return _canMove; }

	#endregion Event Responses

}
