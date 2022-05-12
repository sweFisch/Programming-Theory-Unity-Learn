using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    Transform _transform;

    float _screenExtent = 20f;

    [SerializeField] bool _isMovingRight = false;

    private void Start()
    {
        _transform = transform;
        
        //Debug.Log($" {transform.name } is moving  { transform.forward}");
        if(transform.forward.x > 0f)
        {
            _isMovingRight = true;
        }//else moving left

    }

    private void Update()
    {
        _transform.Translate(Vector3.forward * _speed * Time.deltaTime);

        CheckIfOutsideBounds();
    }

    private void CheckIfOutsideBounds()
    {
        if (_isMovingRight && _transform.position.x > _screenExtent)
        {
            _transform.position = new Vector3(-_screenExtent, _transform.position.y, transform.position.z);
        }
        if (!_isMovingRight && _transform.position.x < -_screenExtent)
        {
            _transform.position = new Vector3(_screenExtent, _transform.position.y, transform.position.z);
        }
    }

    public float GetSpeed()
    {
        return _speed;
    }
}
