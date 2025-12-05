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
    public Vector3 spawnPoint = new Vector3(4, 0, 0);//enemy's origin point, movement will center around
    protected abstract float movementDistance {get;}//how far enemy will move from spawnPoint
    protected abstract float movementSpeed {get;}
    public Vector3 currentDirection = new Vector3(-1, 0, 0);//currently moving left or right?

    protected GameObject player;
    private List<RaycastHit2D> castResult = new List<RaycastHit2D>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        currentHP = maxHP;
        if (EnemyManager.Instance != null) {
            EnemyManager.Instance.RegisterEnemy(this);
        }
        player = GameObject.FindWithTag("Player");//assign player to player variable
    }

    public void Move(float timeChange, Vector3 spawnPoint){
        Vector3 movement = currentDirection * movementSpeed * timeChange;

        //check if movement will be too far
        if (Mathf.Abs(transform.position.x + movement.x - spawnPoint.x) > movementDistance) {
            //only move up to boundary
            Vector3 endPoint = spawnPoint + currentDirection * movementDistance;
            transform.Translate(endPoint - transform.position);
            //flip direction
            currentDirection *= -1;
        }
        else{
            //move full distance
            transform.Translate(movement);
        }

    }
    
    public float DetectPlayer(){
        //cast in current direction (infinite)
        //if Player is found, return distance to player
        //if no player found, return -1

        //debug - do I need to reset the castResult list before casting? it seems not

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

    public void CreatePrefab(GameObject prefab, Vector3 spawnLocation){
        //needs to be this object's child so projectile can find direction it needs to go in
        GameObject createdProjectile = Instantiate(prefab, spawnLocation, Quaternion.identity, gameObject.transform);
    }

    public void CreateProjectile(GameObject projectile){
        //slightly offset it from this, and make this object its parent
        CreatePrefab(projectile, transform.position + new Vector3(currentDirection.x * 0.5f, 0, 0));
    }


    protected void PerformMeleeAttack(GameObject meleeAttack, float meleeAttackDamage, float meleeOffset){
        Debug.Log("Meleeing");

        GameObject meleeAttackIndicator = Instantiate(meleeAttack, transform.position + meleeOffset * currentDirection, Quaternion.identity);
        meleeAttackIndicator.GetComponent<BasicEnemyMelee>().damage = meleeAttackDamage;

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

    public virtual void Die()
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
