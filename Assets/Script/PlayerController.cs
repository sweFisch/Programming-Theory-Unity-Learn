using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _frogSpeed = 5f;
    private float _currentSpeed; // the speed the frog is moving + eventual plattform speed

    [SerializeField] private LayerMask _layerMaskBlock; // blocking objects should be on this layer mask
    [SerializeField] private LayerMask _layerMaskTerrain;
    [SerializeField] private float _detectionRadius = 1f;

    [SerializeField] Transform _transformDebugtarget; // Debug purpose

    private Vector2 _moveInput;
    private Vector3 _moveDirection;

    private Vector3 _destination;
    private Quaternion _lookDirection;
    private float rotationSpeed = 2000f;

    private Transform _transform;

    private PlayerAnimationController _playerAnimationController;
    private PlayerDeath _playerDeath;

    private Transform _currentMovingBlock;
    private float _currentMovingBlockSpeed;

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
        //Debug.Log(_moveInput);
    }

    private void Awake()
    {
        _transform = transform;
        _playerAnimationController = GetComponent<PlayerAnimationController>();
        _playerDeath = GetComponent<PlayerDeath>();

    }

    void Start()
    {
        _destination = _transform.position;
        _currentSpeed = _frogSpeed;
    }

    void Update()
    {

        if (_playerDeath.IsAlive == false) { return; } // Dont allow input if player is dead

        HandleInputSetDestination();
        DebugMovementDestinationUpdatePosition();

        HandleMovingPlattformDestinations();

        HandlePlayerMovemet();

        RotatePlayerInDirectionOfMovement();
    }

    private void HandlePlayerMovemet()
    {
        float distanceFromTarget = Vector3.Distance(_transform.position, _destination);
        if (distanceFromTarget < 0.05f) //Mathf.Epsilon
        {
            _playerAnimationController.EndJumpAnimation();

            CheckLandingSurface(_transform.position); // check the landing surface

            if (distanceFromTarget < Mathf.Epsilon)
            {
                //_transform.position = Vector3.MoveTowards(_transform.position, _destination, Time.deltaTime * _currentSpeed);


            }
            
        }
        else
        {
            _transform.position = Vector3.MoveTowards(_transform.position, _destination, Time.deltaTime * _currentSpeed);
        }
    }

    private void HandleMovingPlattformDestinations()
    {
        float distanceFromTarget = Vector3.Distance(_transform.position, _destination);
        if (_currentMovingBlock != null)
        {
            //_currentSpeed = _currentMovingBlockSpeed; // on�digt ?? kanske
            _destination = _currentMovingBlock.position;

            if (distanceFromTarget < 0.05f)
            {
            _transform.position = _currentMovingBlock.position;
            }
        }
        else
        {
            _currentSpeed = _frogSpeed;
        }
    }

    private void DebugMovementDestinationUpdatePosition()
    {
        // DEBUG MOVE TARGET
        if (_transformDebugtarget != null) { _transformDebugtarget.position = _destination; }
    }

    private void HandleInputSetDestination()
    {
        float distanceFromTarget = Vector3.Distance(_transform.position, _destination);

        if(distanceFromTarget > 0.1f) { return; } //if not close enough to target return

        if (_moveDirection != Vector3.zero && CheckDirection(_moveDirection)) // blocks player from jumping into block terrain
        {
            _destination += _moveDirection;
            SaveRotationOfPlayerInput();

            if (CheckIfMovingPlattform(_destination))
            {
                _playerAnimationController.TriggerJumpAnimation(); // TODO this is not working 
            }
            else if (_currentMovingBlock == null)
            {
                _destination = RoundVector3(_destination); // round to whole numbers
            }

            if (_currentMovingBlock == null)
            {
                _playerAnimationController.TriggerJumpAnimation();
            }
        }
    }
    private bool CheckDirection(Vector3 direction)
    {
        if (Physics.Raycast(_destination, direction, out RaycastHit hit, _detectionRadius, _layerMaskBlock))
        {
            return false;
        }
        return true;
    }

    private void SaveRotationOfPlayerInput()
    {
        // Rotate the player in the direction of movement Save the direction of movement
        if (_moveDirection != Vector3.zero)
        {
            _lookDirection = Quaternion.LookRotation(_moveDirection);
            //transform.rotation = _lookDirection;
        }
    }

    private void CheckLandingSurface(Vector3 pointToCheck)
    {
        
        if(Physics.Raycast((pointToCheck + (Vector3.up*0.5f)), Vector3.down,out RaycastHit hit, _detectionRadius,_layerMaskTerrain))
        {
            //Debug.Log(hit.collider.name);

            Terrain terrain = hit.collider.GetComponent<Terrain>();
            if (terrain == null) { return; }

            if(terrain.terrainType == TerrainType.Water)
            {
                Debug.Log("Fell into water!");
                _destination = _transform.position;
                _playerDeath.Drowning();

            }
            if(terrain.terrainType == TerrainType.MovingBlock)
            {
                _currentMovingBlock = terrain.transform;
                _currentMovingBlockSpeed = terrain.GetComponent<Mover>().GetSpeed() + _frogSpeed;
                Debug.Log("Standing on moving block");
            }
        }
    }

    private bool CheckIfMovingPlattform(Vector3 pointToCheck)
    {
        if (Physics.Raycast((pointToCheck + (Vector3.up * 0.5f)), Vector3.down, out RaycastHit hit, _detectionRadius, _layerMaskTerrain))
        {
            Terrain terrain = hit.collider.GetComponent<Terrain>();
            if (terrain == null) 
            {
                _currentMovingBlock = null;
                return false; 
            }

            if (terrain.terrainType == TerrainType.MovingBlock)
            {
                _currentMovingBlock = terrain.transform;
                
                _currentMovingBlockSpeed = terrain.GetComponent<Mover>().GetSpeed() + _frogSpeed;
                //Debug.Log("Standing on moving block");
                return true;
            }
        }
        _currentMovingBlock = null;
        return false;
    }


    private void RotatePlayerInDirectionOfMovement()
    {
        // smoother rotation of the player _lookDirection is set at input detection
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookDirection, Time.deltaTime * 40f);

        //float deegreesPerSecond = rotationSpeed * Time.deltaTime;
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookDirection, deegreesPerSecond);
    }



    private Vector3 RoundVector3(Vector3 inputVector)
    {
        inputVector = new Vector3(Mathf.Round(inputVector.x), Mathf.Round(inputVector.y), Mathf.Round(inputVector.z));
        return inputVector;
    }

    public void ResetPlayer()
    {
        _playerAnimationController.ResetPlayerAnimations();
        _destination = Vector3.zero;
        _transform.position = Vector3.zero;
    }

}
