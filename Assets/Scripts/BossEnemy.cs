using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossEnemy : Enemy
{
    float timekeeper = 0f;//this will count how much time has passed (in s) since enemy has shot a projectile
    
    [Header ("Attack settings")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject AoEAttack;

    [SerializeField] private float AoECooldown = 5f;
    [SerializeField] private float projectileCooldown = 2f;
    [SerializeField] private float meleeCooldown = 1.5f;
    [SerializeField] private float overallAttackCooldown = 5f;

    [SerializeField] private float AoEDetectionDistance = 10f;
    [SerializeField] private float projectileDetectionDistance = 7f;
    [SerializeField] private float meleeDetectionDistance = 1f;

    [Header("Health Settings")]
    public override float maxHP { get {return 300f;}}
    public float currentHP;

    [Header ("Position settings")]
    //current direction and castResult defined in parent class (Enemy)

    protected override Vector3 spawnPoint {get {return new Vector3(4, -2, 0);}} //enemy's origin point, movement will center around
    protected override float movementDistance {get {return 0f;}}//how far enemy will move from spawnPoint
    protected override float movementSpeed {get {return 0f;}}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = maxHP;
        if (EnemyManager.Instance != null) {
            EnemyManager.Instance.RegisterEnemy(this);
        }
    }
    

    // Update is called once per frame
    void Update()
    {

        timekeeper += Time.deltaTime;//increase timer every update
        
        //if player is w/in distance, shoot 'em!
        // if (gameObject.GetComponent<Collider2D>().Cast(currentDirection, castResult, detectionDistance) > 0){

        //     bool foundPlayer = false;
        //     foreach (RaycastHit2D hitItem in castResult){
        //         if (hitItem.transform.gameObject.tag == "Player"){
        //             foundPlayer = true;
        //             break;
        //         }
        //     }

        //     //if it's been enough time, instantiate a projectile
        //     //slightly offset it from this, and make this object its parent
        //     //needs to be this object's child so projectile can find direction it needs to go in
        //     if (foundPlayer && timekeeper >= shootInterval){
        //         timekeeper = 0f;
        //         Instantiate(projectile, transform.position + new Vector3(currentDirection.x * 0.5f, 0, 0), Quaternion.identity, gameObject.transform);
        //     }
        // }

    }
    
    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        Debug.Log("Enemy took " + damage + " damage. HP: " + currentHP);
        
        if (currentHP <= 0.1)
        {
            Die();
        }
    }

    void Die()
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
