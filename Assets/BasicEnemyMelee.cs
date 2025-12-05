using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicEnemyMelee : MonoBehaviour
{

    [SerializeField] private float indicatorRadius = 0.1f;
    [SerializeField] private float secAttackDelay = 0.1f;
    [SerializeField] private float secAttackDuration = 0.1f;
    public float damage = 10f;

    private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {   
        player = GameObject.FindWithTag("Player");
        StartCoroutine(Attack());
    }

    private IEnumerator Attack(){
        //wait first
        yield return new WaitForSeconds(secAttackDelay);

        //then attack
        //visual indicator
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 255f); // easier to time things if changed in code rather than anim

        //see if player is close enough to be hit
        bool playerHit = false;

        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= indicatorRadius){
            playerHit = true;
        }

        StartCoroutine(Disappear());

        if (playerHit){
            player.GetComponent<Player>().ApplyDamage(damage);
        }
    }

    private IEnumerator Disappear(){
        //wait animation length
        yield return new WaitForSeconds(secAttackDuration);

        //disappear
        Destroy(gameObject);
    }
}
