using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    Transform _transform;

    private void Start()
    {
        _transform = transform;
    }

    private void Update()
    {
        _transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
