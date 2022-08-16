using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogColorStation : Interaction
{
    // OBS need to set the detection layerMask!

    // INHERITHANCE
    public override void Interact()
    {
        if(!hasInteracted)
        {
            ChangeColor();
            hasInteracted = true;
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
                Debug.Log("Random Color");
            }
        }

        PlayerGFX playerGFX = _player.GetComponent<PlayerGFX>();
        if (playerGFX != null)
        {
            _player.GetComponent<PlayerGFX>().ChangePlayerColor(newColor);
        }

    }

    public Color RandomColor()
    {
        Color randomColor = new Color(Random.value, Random.value, Random.value);
        return randomColor;
    }
}
