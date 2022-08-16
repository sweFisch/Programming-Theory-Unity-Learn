using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpinner : MonoBehaviour
{

    [SerializeField] private float _rotationSpeed = 35f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, Time.deltaTime * _rotationSpeed, 0));
    }
}
