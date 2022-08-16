using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGFX : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] primaryColorMeshesArray;

    public void ChangePlayerColor(Color color)
    {
        //MeshRenderer[] meshRenders = transform.GetComponentsInChildren<MeshRenderer>(); // old code
        if (primaryColorMeshesArray.Length > 0)
        {
            foreach (var meshRenderer in primaryColorMeshesArray)
            {
                meshRenderer.material.SetColor("_BaseColor", color);
                Debug.Log("Random Color");
            }
        }
    }
}
