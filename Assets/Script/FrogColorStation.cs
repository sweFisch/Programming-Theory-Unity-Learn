using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogColorStation : Interaction
{
    // OBS need to set the detection layerMask!

    private float interactionCooldown = 0.5f;
    private float timeWhenNextInteractionCanOccur;

    private void Start()
    {
        timeWhenNextInteractionCanOccur = Time.time;
    }

    // INHERITHANCE
    public override void Interact()
    {
        if(Time.time > timeWhenNextInteractionCanOccur)
        {
            hasInteracted = false;
        }

        if(!hasInteracted)
        {
            ChangeColor();
            hasInteracted = true;
            timeWhenNextInteractionCanOccur = Time.time + interactionCooldown;
        }
        //base.Interact();
    }

    private void FixedUpdate()
    {
        DetectPlayer();
    }

    public void ChangeColor()
    {
        Color newColor = RandomColor();

        MeshRenderer[] meshRenders = transform.GetComponentsInChildren<MeshRenderer>();
        if(meshRenders.Length > 0)
        {
            foreach (var meshRenderer in meshRenders)
            {
                meshRenderer.material.SetColor("_BaseColor", newColor);
            }
            Debug.Log("Random Color");
        }

        // send the color to the player
        if (_player != null)
        {
            PlayerGFX playerGFX = _player.GetComponent<PlayerGFX>();
            if (playerGFX != null)
            {
                _player.GetComponent<PlayerGFX>().ChangePlayerColor(newColor);
            }
        }
    }

    public Color RandomColor()
    {
        Color randomColor = new Color(Random.value, Random.value, Random.value);
        return randomColor;
    }
}
