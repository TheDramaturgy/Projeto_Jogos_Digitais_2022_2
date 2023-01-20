using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour {
    private Camera _camera;
    private NavMeshAgent _agent;
    private Transform _transform;
    private Animator _animator;
	private float _interactionRange;
    private bool _isLookingLeft;
	private bool _isMoving;

    [SerializeField] private GameObjectVariable _clickedItem;
    [SerializeField] private TextDictionary _itemComments;
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

	public void MoveCharacterToClickedItem() {
        _moveTarget.Value = _clickedItem.Value.transform.position;
        _moveObjective.Value = "ItemPickup";
        _moveTargetSetEvent.Invoke();
    }

	public void MoveCharacterToClickedItem(float range, UnityAction callback) {
		if (Vector3.Distance(_clickedItem.Value.transform.position, transform.position) > _interactionRange) {
			_interactionRange = range;
			_moveTargetReachEvent.AddListener(callback);
			_moveObjective.Value = "ItemPickup";
			_moveTarget.Value = _clickedItem.Value.transform.position;
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

	#endregion Event Responses

}
