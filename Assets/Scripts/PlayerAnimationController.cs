using UnityEngine;
using System.Diagnostics;

public class PlayerAnimationController : MonoBehaviour
{

    [Header("Animator")]
    public Animator animator;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void playAttackAnim()
    {
        animator.SetTrigger("TriggerAttack");
    }
    
    public void playDeathAnim()
    {
        animator.SetTrigger("TriggerDeath");
    }

    public void playHitAnim()
    {
        animator.SetTrigger("TriggerHit");
    }

    public void setIsWalking(bool val)
    {
        animator.SetBool("isWalking", val);
    }

}
