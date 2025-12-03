using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntermediateEnemy : Enemy
{
    float timekeeper = 0f;//how much time has passed since performing any attack
    float projectileTimekeeper = 0f;//this will count how much time has passed (in s) since enemy has shot a projectile
    float meleeTimekeeper = 0f;//how much time has passed (in s) since enemy has performed melee attack
    float dashTimekeeper = 0f;//same for dash attack
    
    [SerializeField] private GameObject projectile;
    [SerializeField] private float meleeAttackDamage = 20f;

    [Header ("Cooldowns")]
    [SerializeField] private float overallAttackCooldown = 1f;
    [SerializeField] private float meleeCooldown = 4f;
    [SerializeField] private float projectileCooldown = 1.5f;
    [SerializeField] private float dashCooldown = 3f;

    [Header ("Detection Distances")]
    [SerializeField] private float projectileDetectionDistance = 8f;
    [SerializeField] private float meleeDetectionDistance = 3f;
    [SerializeField] private float dashDetectionDistance = 5f;

    [Header("Health Settings")]
    public override float maxHP { get {return 100f;}}

    [Header ("Move settings")]
    [SerializeField] private Vector3 spawnPoint = new Vector3(4, 0, 0); //enemy's origin point, movement will center around
    protected override float movementDistance {get {return 2f;}}//how far enemy will move from spawnPoint
    protected override float movementSpeed {get {return 1f;}}
    [SerializeField] private float dashStep = 4f;//movement speed per frame when dashing

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

        timekeeper += Time.deltaTime;//increase timers every update
        projectileTimekeeper += Time.deltaTime;
        meleeTimekeeper += Time.deltaTime;
        dashTimekeeper += Time.deltaTime;
        
        //if player is w/in distance, shoot 'em!
        float playerDistance = DetectPlayer();

        //if it spots player, go through attack logic
        if (playerDistance >= 0 && playerDistance <= projectileDetectionDistance){

            if (timekeeper >= overallAttackCooldown){
                //attack only if have not attacked within overall cooldown

                //melee
                if (playerDistance >= 0 && playerDistance <= meleeDetectionDistance){
                    //perform melee attack
                    if (meleeTimekeeper >= meleeCooldown){
                        timekeeper = 0f;
                        // Debug.Log("Melee attack!");
                        PerformMeleeAttack(meleeDetectionDistance, meleeAttackDamage);
                    }
                }

                //mid-range (dash attack)
                else if (playerDistance <= dashDetectionDistance){
                    if (dashTimekeeper >= dashCooldown){
                        timekeeper = 0f;
                        dashTimekeeper = 0f;//reset timekeeper - other attacks do this in function definition
                        // Debug.Log("Dash attack!");
                        PerformDashAttack();
                    }
                }
                
                //ranged (projectile attack)
                else if (playerDistance <= projectileDetectionDistance){
                    if (projectileTimekeeper >= projectileCooldown){
                        timekeeper = 0f;
                        // Debug.Log("Shoot!");
                        CreateProjectile(projectile);
                    }
                }

                //if >10 or <1 (-1 for if it can't see the player), then do nothing
            }

        }
        //no player, move back and forth
        else{
            Move(Time.deltaTime, spawnPoint);
        }
    }

    private void PerformDashAttack(){
        float playerPosition = player.transform.position.x;
        Vector3 movement = new Vector3(playerPosition - transform.position.x, 0f, 0f);

        //move spawn point
        spawnPoint += movement;

        //go to player x position
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerPosition, transform.position.y, 0f), dashStep);

        //do melee attack
        PerformMeleeAttack(meleeDetectionDistance, meleeAttackDamage);
    }

}
