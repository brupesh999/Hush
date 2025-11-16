using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicEnemy : MonoBehaviour
{
    float timekeeper = 0f;//this will count how much time has passed (in s) since enemy has shot a projectile
    
    [Header ("Shooty settings")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private float detectionDistance = 7f;

    [Header ("Move-y settings")]
    [SerializeField] private Vector3 spawnPoint = new Vector3(4, -2, 0);//enemy's origin point, movement will center around
    [SerializeField] private float movementDistance = 2f;//how far enemy will move from spawnPoint
    [SerializeField] private float movementSpeed = 1f;
    public Vector3 currentDirection = new Vector3(-1, 0, 0);//currently moving left or right?

    private List<RaycastHit2D> castResult = new List<RaycastHit2D>();

    // Update is called once per frame
    void Update()
    {
        timekeeper += Time.deltaTime;//increase timer every update
        
        //if player is w/in distance, shoot 'em!
        if (gameObject.GetComponent<Collider2D>().Cast(currentDirection, castResult, detectionDistance) > 0){

            bool foundPlayer = false;
            foreach (RaycastHit2D hitItem in castResult){
                if (hitItem.transform.gameObject.tag == "Player"){
                    foundPlayer = true;
                    break;
                }
            }

            //if it's been enough time, instantiate a projectile
            //slightly offset it from this, and make this object its parent
            //needs to be this object's child so projectile can find direction it needs to go in
            if (foundPlayer && timekeeper >= shootInterval){
                timekeeper = 0f;
                Instantiate(projectile, transform.position + new Vector3(currentDirection.x * 0.5f, 0, 0), Quaternion.identity, gameObject.transform);
            }
        }

        //no player, move back and forth
        else{
            transform.Translate(currentDirection * movementSpeed * Time.deltaTime);
            if (Mathf.Abs(transform.position.x - spawnPoint.x) > movementDistance) currentDirection *= -1;
        }
    }
}
