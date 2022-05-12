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
    [SerializeField] GameObject gfxMesh;
    [SerializeField] ParticleSystem _drowningParticles;

    private void Start()
    {
        IsAlive = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsAlive) { return; }

        if(other.CompareTag(CAR))
        {
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
        _playerAnimationController.TriggerDeathOverRunAnimation();
        TakeLife();
    }

    public void JumpIntoCar()
    {
        // play animation to make it stick to side
        TakeLife();
    }

    public void Drowning()
    {
        // play animation for drowning - TODO make drowning animation
        //_playerAnimationController.TriggerDeathOverRunAnimation();
        gfxMesh.SetActive(false);
        _drowningParticles.Play();
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
        IsAlive = true;
        gfxMesh.SetActive(true);
    }

}
