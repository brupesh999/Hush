using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicEnemy : MonoBehaviour
{
    float timekeeper = 0f;//this will count how much time has passed (in s) since enemy has shot a projectile
    [SerializeField] private GameObject projectile;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private float detectionDistance = 7f;

    private List<RaycastHit2D> castResult = new List<RaycastHit2D>();

    // Update is called once per frame
    void Update()
    {
        timekeeper += Time.deltaTime;//increase timer every update
        
        if (gameObject.GetComponent<Collider2D>().Cast(new Vector2(-1, 0), castResult, detectionDistance) > 0){

            bool foundPlayer = false;
            foreach (RaycastHit2D hitItem in castResult){
                if (hitItem.transform.gameObject.tag == "Player"){
                    foundPlayer = true;
                    break;
                }
            }

            if (foundPlayer && timekeeper >= shootInterval){
                timekeeper = 0f;
                Instantiate(projectile, transform.position + new Vector3(-0.5f, 0, 0), Quaternion.identity);
            }
        }
    }
}
