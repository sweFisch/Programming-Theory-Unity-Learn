using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 7f;

    [SerializeField] private LayerMask _layerMaskBlock; // blocking objects should be on this layer mask
    [SerializeField] private LayerMask _layerMaskTerrain;
    [SerializeField] private float _detectionRadius = 1f;

    [SerializeField] Transform _transformDebugtarget; // Debug purpose

    private Vector2 _moveInput;
    private Vector3 _moveDirection;

    private Vector3 _destination;
    private Quaternion _lookDirection;
    //private float rotationSpeed = 2000f;

    private Transform _transform;

    private PlayerAnimationController _playerAnimationController;
    private PlayerDeath _playerDeath;

    private Transform _currentMovingBlock;

    private float _distanceLeeway = 0.05f;

    [SerializeField] private float _lerpTime = 0f;
    private Vector3 _lerpStartPos;

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
        _lerpStartPos = transform.position;
    }

    void Update()
    {

        if (_playerDeath.IsAlive == false) { return; } // Dont allow input if player is dead

        HandleInputSetDestination();

        DebugMovementDestinationUpdatePosition();

        HandleMovingPlattformDestinations();

        HandlePlayerMovemet();

        RotatePlayerInDirectionOfMovement();

        HandleInteraction();
    }

    private void HandleInteraction()
    {
        if(!CheckDirection(_moveDirection))
        {
            TryToInteract();
        }
    }

    private void TryToInteract()
    {
        if (Physics.Raycast(_transform.position, _moveDirection, out RaycastHit hit, _detectionRadius, _layerMaskBlock))
        {
            Interaction interactionObj = hit.collider.GetComponent<Interaction>();
            if(interactionObj != null)
            {
                interactionObj.Interact();
            }
        }
    }

    private void HandleInputSetDestination()
    {
        float distanceFromTarget = Vector3.Distance(_transform.position, _destination);

        if (distanceFromTarget > _distanceLeeway) { return; } //if not close enough to target return

        SaveRotationOfPlayerInput();

        if (_moveDirection != Vector3.zero && CheckDirection(_moveDirection)) // blocks player from jumping into block terrain
        {
            _destination += _moveDirection;
            //SaveRotationOfPlayerInput();

            if (CheckIfMovingPlattform(_destination))
            {
                _playerAnimationController.TriggerJumpAnimation();
            }
            else if (_currentMovingBlock == null)
            {
                _playerAnimationController.TriggerJumpAnimation();
                _destination = RoundVector3(_destination); // round to whole numbers if not on a moving plattform
            }

        }
        // Reset the Lerp Time to start a new movement
        _lerpTime = 0f;
        _lerpStartPos = transform.position;

    }
    private bool CheckDirection(Vector3 direction)
    {
        if (Physics.Raycast(_destination, direction, out RaycastHit hit, _detectionRadius, _layerMaskBlock))
        {
            return false;
        }
        return true;
    }

    private void DebugMovementDestinationUpdatePosition()
    {
        // DEBUG MOVE TARGET
        if (_transformDebugtarget != null) { _transformDebugtarget.position = _destination; }
    }

    private void HandleMovingPlattformDestinations()
    {
        float distanceFromTarget = Vector3.Distance(_transform.position, _destination);
        if (_currentMovingBlock != null)
        {
            _destination = _currentMovingBlock.position;

            if (distanceFromTarget < _distanceLeeway)
            {
                _transform.position = _currentMovingBlock.position;
            }
        }
    }

    private void HandlePlayerMovemet()
    {
        float distanceFromTarget = Vector3.Distance(_transform.position, _destination);
        _lerpTime += Time.deltaTime * _movementSpeed;
        _lerpTime = Mathf.Clamp(_lerpTime, 0f, 1f);

        if(distanceFromTarget < 0.1f)
        {
            _playerAnimationController.EndJumpAnimation();
            CheckLandingSurface(_transform.position); // check the landing surface
        }

        if(distanceFromTarget > 0.01f)
        {
            //transform.position = Vector3.Lerp(transform.position, _destination, _lerpTime); // not liniar movement ,ease out 
            transform.position = Vector3.Lerp(_lerpStartPos, _destination, _lerpTime);
        }
        else
        {
            transform.position = _destination;
            CheckLandingSurface(_transform.position); // check the landing surface
        }
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
                //Debug.Log("Fell into water!");
                _destination = _transform.position;
                _playerDeath.Drowning();

            }
            if(terrain.terrainType == TerrainType.MovingBlock)
            {
                _currentMovingBlock = terrain.transform;
                //Debug.Log("Standing on moving block");
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
