using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    Transform _transform;

    float screenExtent = 20f;

    [SerializeField] bool isMovingRight = false;

    private void Start()
    {
        _transform = transform;
        
        //Debug.Log($" {transform.name } is moving  { transform.forward}");
        if(transform.forward.x > 0f)
        {
            isMovingRight = true;
        }//else moving left

    }

    private void Update()
    {
        _transform.Translate(Vector3.forward * speed * Time.deltaTime);

        CheckIfOutsideBounds();
    }

    private void CheckIfOutsideBounds()
    {
        if (isMovingRight && _transform.position.x > screenExtent)
        {
            _transform.position = new Vector3(-screenExtent, _transform.position.y, transform.position.z);
        }
        if (!isMovingRight && _transform.position.x < -screenExtent)
        {
            _transform.position = new Vector3(screenExtent, _transform.position.y, transform.position.z);
        }
    }
}
