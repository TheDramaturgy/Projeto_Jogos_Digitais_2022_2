using JetBrains.Annotations;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour {
    private Camera _camera;
    private NavMeshAgent _agent;
    private Transform _transform;
    private Animator _animator;
    private bool _isLookingLeft;
	private bool _isMoving;

    /*[SerializeField] private ClickedItem _clickedItem;
    [SerializeField] private StringVariable _moveObjective;*/

    [SerializeField] private UnityEvent _moveTargetSetEvent;
	[SerializeField] private UnityEvent _moveTargetReachEvent;
	[SerializeField] private UnityEvent _moveCancelEvent;
	[SerializeField] private MoveTarget _moveTarget;

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

    void Update() {
        // Check if left mouse button has been clicked
        if (Input.GetMouseButtonUp(0)) {
			var ray = _camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit)) {
                if (hit.transform.gameObject.tag == "Walkable") {
                    _moveTarget.Destination = hit.point;
                    //_moveObjective.Value = "ChangePosition";
                    _moveCancelEvent.Invoke();
                    _moveTargetSetEvent.Invoke();
                }
			}
		}

		// Check if character is moving
		if (_agent.velocity.magnitude != 0 && !_isMoving) {
            _isMoving = true;
        } else if (_agent.velocity.magnitude == 0 && _isMoving) {
            _isMoving = false;
            var target = new Vector3(_moveTarget.Destination.x, 0.0f, _moveTarget.Destination.z);
            var reach = new Vector3(gameObject.transform.position.x, 0.0f, gameObject.transform.position.z);
            if (target == reach) {
                _moveTargetReachEvent.Invoke();
            }
        }

		HandleAnimation();
		_transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
    }

	// ------ Auxiliary Methods ------

	public void HandleMovement() {
        _agent.destination = _moveTarget.Destination;
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

    /*
    public void MoveCharacterToClickedItem() {
        _moveTarget.Destination = _clickedItem.ItemGameObject.transform.position;
        _moveObjective.Value = "ItemPickup";
        _moveTargetSetEvent.Invoke();
    }
    */

}
