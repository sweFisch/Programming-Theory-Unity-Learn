using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] GameObject _prefabGridObj;

    [SerializeField] Vector2 gridsize;
    private void Start()
    {
        for (int x = 0; x < gridsize.x; x++)
        {
            for (int y = 0; y < gridsize.y; y++)
            {
                Vector3 gridPos = new Vector3(x,0,y);
                Instantiate(_prefabGridObj, gridPos, Quaternion.identity, transform);
            }
        }
    }
}
