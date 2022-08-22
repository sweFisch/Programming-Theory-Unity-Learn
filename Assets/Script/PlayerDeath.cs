using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{

    // Encapsulation - Getters and setters
    public bool IsAlive { get; private set; }

    private const string CAR = "Car";
    private const string CROCODILE = "Crocodile";
    private const string SPIKE = "Spike";

    [SerializeField] PlayerAnimationController _playerAnimationController;
    [SerializeField] GameObject gfxMesh;
    [SerializeField] ParticleSystem _drowningParticles;
    [SerializeField] ParticleSystem _bloodParticles;

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
            // Death is handeld by Terrain Type landing surface
            //DeathBySpikes();
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

    public void DeathBySpikes()
    {
        _bloodParticles.Play();
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
        MainManager mainManager = MainManager.Instance;
        if(mainManager != null)
        {
            mainManager.SaveCurrentPlayerScore(); // try to update the current session score
        }

        Debug.Log("Player Lost Life , Waiting 2 sek, before restart");
        yield return new WaitForSeconds(2f);
        GetComponent<PlayerController>().ResetPlayer();
        GetComponent<PlayerScore>().ResetScore(); // Resets the score, if Lives save and add scores

        IsAlive = true;
        gfxMesh.SetActive(true);

        //TODO Scene flow Lives ??
        if (mainManager != null)
        {
            mainManager.GoToMainMenue();
        }
    }

}
