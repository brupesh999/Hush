using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AoEAttack : MonoBehaviour
{

    [SerializeField] private float secAttackDelay = 3f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float secAnimationLength = 1f;//DEBUG - not sure if this will be needed once an animation is added
    [SerializeField] private float damage = 15f;

    private List<RaycastHit2D> castResult = new List<RaycastHit2D>();

    void Awake(){
        //should be instantiated below the player position, on ground

        //wait secAttackDelay seconds, then attack player
        StartCoroutine(Attack());
    }

    private IEnumerator Attack(){
        //wait first
        yield return new WaitForSeconds(secAttackDelay);

        //then attack
        //visual indicator
        gameObject.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 55f);//debug - this shouldn't be necessary after animations added

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
        yield return new WaitForSeconds(secAnimationLength);

        //disappear
        Destroy(gameObject);
    }
}
