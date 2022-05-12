using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] Animator _animator;

    public void TriggerJumpAnimation()
    {
        _animator.SetBool("Jump",true);
    }
    public void EndJumpAnimation()
    {
        _animator.SetBool("Jump", false);
    }

    public void TriggerDeathOverRunAnimation()
    {
        _animator.SetTrigger("DeathOverRun");
    }

    public void ResetPlayerAnimations()
    {
        _animator.SetBool("Jump", false);
        _animator.ResetTrigger("DeathOverRun");
        _animator.Play("PlayerIdle", -1, 0f);
    }

}
