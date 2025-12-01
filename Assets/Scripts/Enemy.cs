using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Enemy : MonoBehaviour
{
    [Header("Health Settings")]
    // public float maxHP = 50f;
    public abstract float maxHP {get;}
    public float currentHP { get; protected set; }

    [Header ("Move-y settings")]
    protected abstract Vector3 spawnPoint {get;}//enemy's origin point, movement will center around
    protected abstract float movementDistance {get;}//how far enemy will move from spawnPoint
    protected abstract float movementSpeed {get;}
    public Vector3 currentDirection = new Vector3(-1, 0, 0);//currently moving left or right?

    private List<RaycastHit2D> castResult = new List<RaycastHit2D>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        currentHP = maxHP;
        if (EnemyManager.Instance != null) {
            EnemyManager.Instance.RegisterEnemy(this);
        }
    }

    public void Move(float timeChange){
        //move appropriate distance
        transform.Translate(currentDirection * movementSpeed * timeChange);
        //if moved past boundary, flip
        if (Mathf.Abs(transform.position.x - spawnPoint.x) > movementDistance) currentDirection *= -1;
    }
    
    public float DetectPlayer(){
        //cast in current direction (infinite)
        //if Player is found, return distance to player
        //if no player found, return -1

        //debug - do I need to reset the castResult list before casting?

        bool foundPlayer = false;
        GameObject player = gameObject;
        if (gameObject.GetComponent<Collider2D>().Cast(currentDirection, castResult) > 0){

            foreach (RaycastHit2D hitItem in castResult){
                if (hitItem.transform.gameObject.tag == "Player"){
                    foundPlayer = true;
                    player = hitItem.transform.gameObject;
                    break;
                }
            }
        }

        if (foundPlayer){
            return Vector3.Distance(player.transform.position, gameObject.transform.position);
        }

        else{
            return -1f;
        }
        
    }

    public void CreateProjectile(GameObject projectile){
        //slightly offset it from this, and make this object its parent
        //needs to be this object's child so projectile can find direction it needs to go in
        GameObject createdProjectile = Instantiate(projectile, transform.position + new Vector3(currentDirection.x * 0.5f, 0, 0), Quaternion.identity, gameObject.transform);
        //createdProjectile.GetComponent<BasicEnemyProjectile>().SetActive(true);
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        //Debug.Log("Enemy took " + damage + " damage. HP: " + currentHP);
        
        if (currentHP <= 0.1)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Enemy died!");

        if (EnemyManager.Instance != null) {
            EnemyManager.Instance.UnregisterEnemy(this);
        }

        //move child projectiles before destroying
        foreach(Transform child in gameObject.GetComponentsInChildren<Transform>()){
            child.gameObject.transform.SetParent(null);
        }

        Destroy(gameObject);
       
    }
}
