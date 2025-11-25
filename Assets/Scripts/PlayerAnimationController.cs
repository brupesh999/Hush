using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{

    [Header("Animator")]
    public Animator animator;

    [Header("Transition Variables")]
    public bool isWalking; // = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }


    void playAttackAnim()
    {
        animator.SetTrigger("TriggerAttack");
    }
    
    void playDeathAnim()
    {
        animator.SetTrigger("TriggerDeath");
    }

    void playHitAnim()
    {
        animator.SetTrigger("TriggerHit");
    }

}
