using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicEnemyMelee : MonoBehaviour
{

    [SerializeField] private float secAttackDelay = 0.1f;
    [SerializeField] private float secAttackDuration = 0.1f;
    [SerializeField] public float damage = 10f;
    [SerializeField] private float attackRange = 3f;
    private List<RaycastHit2D> castResult = new List<RaycastHit2D>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {   
        StartCoroutine(Attack());
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Attack(){
        //wait first
        yield return new WaitForSeconds(secAttackDelay);

        //then attack
        //visual indicator
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 255f); // easier to time things if changed in code rather than anim

        //find player
        bool foundPlayer = false;
        GameObject player = gameObject;

        if (gameObject.GetComponent<Collider2D>().Cast(new Vector3(0, 1, 0), castResult, attackRange) > 1){
            foreach (RaycastHit2D hitItem in castResult){
                if (hitItem.transform.gameObject.tag == "Player"){

                    foundPlayer = true;
                    player = hitItem.transform.gameObject;
                    break;
                }

            }
        }

        StartCoroutine(Disappear());

        if (foundPlayer){
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
