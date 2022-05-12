using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public bool IsAlive { get; set; }

    private const string CAR = "Car";
    private const string CROCODILE = "Crocodile";
    private const string SPIKE = "Spike";

    [SerializeField] PlayerAnimationController _playerAnimationController;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(CAR))
        {
            _playerAnimationController.TriggerDeathOverRunAnimation();
            OverRunByCar();
        }

        if(other.CompareTag(CROCODILE))
        {

        }

        if(other.CompareTag(SPIKE))
        {

        }
    }

    public void OverRunByCar()
    {
        // play animation to make it flat

        TakeLife();
    }

    public void JumpIntoCar()
    {
        // play animation to make it stick to side
        TakeLife();
    }

    public void Drowning()
    {
        // play animation for drowning
        // play particle system
        TakeLife();
    }

    public void TakeLife()
    {
        IsAlive = false;
        // Remove life
        // Wait for animation to finish
        // Reset the player

        StartCoroutine(ResetWaitTime());
        
    }

    IEnumerator ResetWaitTime()
    {
        Debug.Log("Player Lost Life , Waiting 2 sek, before restart");
        yield return new WaitForSeconds(2f);
        GetComponent<PlayerController>().ResetPlayer();
    }

}
