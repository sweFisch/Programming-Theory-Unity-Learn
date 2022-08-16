using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float _interactionDetectionRadius = 1f;
    public LayerMask _interactionLayerMask;
    
    public bool hasInteracted = false;
    public bool hasDetectedPlayer = false;

    Collider[] thingsThatAreClose;

    public PlayerController _player;

    public virtual void Interact()
    {
        if (hasInteracted == false) // only interact with the player once.
        {
            hasInteracted = true;
            Debug.Log($"interacting with {gameObject.name}");
        }
    }

    public virtual void DetectPlayer()
    {
        //if(hasDetectedPlayer == true) { return; } // only detect player once

        thingsThatAreClose = Physics.OverlapSphere(transform.position, _interactionDetectionRadius, _interactionLayerMask);
        if (thingsThatAreClose != null && !hasDetectedPlayer)
        {
            for (int i = 0; i < thingsThatAreClose.Length; i++)
            {
                _player = thingsThatAreClose[i].GetComponent<PlayerController>();
                if(_player != null)
                {
                    Debug.Log($"Player is close {_player.name}");
                    hasDetectedPlayer = true;
                }
            }
        }
        else if(thingsThatAreClose.Length == 0 && hasDetectedPlayer)
        {
            Debug.Log("Player has left the detection Radius");
            hasDetectedPlayer = false;
            hasInteracted = false;
        }
    }

    private void FixedUpdate()
    {
        DetectPlayer();
    }
}
