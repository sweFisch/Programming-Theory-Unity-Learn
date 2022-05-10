using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    [SerializeField] Transform _target;

    private Vector2 _moveInput;
    private Vector3 _moveDirection;

    private Vector3 _destination;

    private Transform _transform;

    public void OnMove(InputAction.CallbackContext context)
    {
        
        _moveInput = context.action.ReadValue<Vector2>();

        // Force binary left, right, up, down. no sidway motion
        if(_moveInput.x > 0.75f || _moveInput.x < -0.75f)
        {
            _moveDirection = new Vector3(_moveInput.x, 0f, 0f).normalized;
        }
        else if(_moveInput.y > 0.75f || _moveInput.y < -0.75f)
        {
            _moveDirection = new Vector3(0f , 0f, _moveInput.y).normalized;
        }
        else
        {
            _moveDirection = Vector3.zero;
        }

        //_moveDirection = new Vector3(_moveInput.x, 0f, _moveInput.y);

        //Debug.Log(_moveInput);

    }

    private void Awake()
    {
        _transform = transform;
    }

    void Start()
    {
        _destination = _transform.position;
    }

    void Update()
    {
        //_transform.Translate(_moveDirection * Time.deltaTime * speed);

        // 0.1f should give some leeway with the timeing of the button press
        float distanceFromTarget = Vector3.Distance(_transform.position, _destination);
        if (distanceFromTarget < 0.1f) //Mathf.Epsilon
        {
            _destination += _moveDirection;

            // DEBUG MOVE TARGET
            _target.position = _destination;

            if (distanceFromTarget < Mathf.Epsilon)
            {
                _transform.position = Vector3.MoveTowards(_transform.position, _destination, Time.deltaTime * speed);
            }
        }
        else
        {
            _transform.position = Vector3.MoveTowards(_transform.position, _destination, Time.deltaTime * speed);
        }
    }
}
