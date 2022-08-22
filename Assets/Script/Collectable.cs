using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    public enum CollectableEnum
    {
        Score,
        Frog,
        Life
    }

    [SerializeField] private CollectableEnum collectableType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collectable picked up");

            switch (collectableType)
            {
                case CollectableEnum.Score:
                    Debug.Log("Score Collected");
                    break;
                case CollectableEnum.Frog:
                    Debug.Log("Frog Collected");
                    break;
                case CollectableEnum.Life:
                    Debug.Log("Life Collected");
                    break;
                default:

                    break;
            }

            Destroy(gameObject);
        }
        
        //Destroy(gameObject);
    }



}
