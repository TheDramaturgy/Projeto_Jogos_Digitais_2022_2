using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class s_PlayerController : MonoBehaviour {
    private Camera _camera;
    private NavMeshAgent _agent;
    private Transform _transform;
    private Animator _animator;
    private bool _IsLookingLeft;


	public Vector3 Speed;

    // Start is called before the first frame update
    void Start() {
        _camera = Camera.main;
        if (_camera == null) {
            Debug.LogError("Main Camera not found by PlayerController.");
        }

        _agent = GetComponent<NavMeshAgent>();
        _transform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();
        _IsLookingLeft = false;
	}

    // Update is called once per frame
    void Update() {
        HandleMovement();
        HandleAnimation();
        _transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
        Speed = _agent.velocity;
    }

    private void HandleMovement() {
        if (!Input.GetMouseButton(0)) return;

        RaycastHit hit;
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) {
            _agent.destination = hit.point;
        }
    }

    private void HandleAnimation() {
        if (_agent.velocity.magnitude != 0 && !_animator.GetBool("isMoving")) {
            _animator.SetBool("isMoving", true);
		} else if (_agent.velocity.magnitude == 0 && _animator.GetBool("isMoving")) {
			_animator.SetBool("isMoving", false);
		}

        if (_agent.velocity.x < 0 && !_IsLookingLeft) {
			_transform.localScale = new Vector3(_transform.localScale.x * -1, _transform.localScale.y, _transform.localScale.z);
            _IsLookingLeft = true;
		} else if (_agent.velocity.x > 0 && _IsLookingLeft) {
			_transform.localScale = new Vector3(_transform.localScale.x * -1, _transform.localScale.y, _transform.localScale.z);
            _IsLookingLeft = false;
		}

	}
}
