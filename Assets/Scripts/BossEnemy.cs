using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossEnemy : Enemy
{
    float timekeeper = 0f;//how much time has passed since performing any attack
    float projectileTimekeeper = 0f;//this will count how much time has passed (in s) since enemy has shot a projectile
    float meleeTimekeeper = 0f;//how much time has passed (in s) since enemy has performed melee attack
    float AoETimekeeper = 0f;//same for AoE attack

    [Header ("Attack settings")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject AoEAttack;

    [SerializeField] private float AoECooldown = 5f;
    [SerializeField] private float projectileCooldown = 2f;
    [SerializeField] private float meleeCooldown = 1.5f;
    [SerializeField] private float overallAttackCooldown = 1f;

    [SerializeField] private float AoEDetectionDistance = 10f;
    [SerializeField] private float projectileDetectionDistance = 7f;
    [SerializeField] private float meleeDetectionDistance = 1f;

    [Header("Health Settings")]
    public override float maxHP { get {return 300f;}}

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
        //increase timers every update
        timekeeper += Time.deltaTime;
        projectileTimekeeper += Time.deltaTime;
        meleeTimekeeper += Time.deltaTime;
        AoETimekeeper += Time.deltaTime;

        float playerDistance = DetectPlayer();

        //attacks are split both by player distance and by attack cooldown

        if (timekeeper >= overallAttackCooldown){
            //attack only if have not attacked within overall cooldown

            //melee - 0 to 1
            if (playerDistance >= 0 && playerDistance <= meleeDetectionDistance){
                //perform melee attack
                if (meleeTimekeeper >= meleeCooldown){
                    timekeeper = 0f;
                    PerformMeleeAttack();
                }
            }

            //mid-range (projectile attack) - 1 to 7
            else if (playerDistance <= projectileDetectionDistance){
                if (projectileTimekeeper >= projectileCooldown){
                    timekeeper = 0f;
                    projectileTimekeeper = 0f;//reset timekeeper - other attacks do this in function definition
                    CreateProjectile(projectile);
                }
            }
            
            //ranged (AoE attack) - 7 to 10
            //DEBUG - currently no projectile for ranged
            else if (playerDistance <= AoEDetectionDistance){
                if (AoETimekeeper >= AoECooldown){
                    timekeeper = 0f;
                    PerformAoEAttack();
                }
            }

            //if >10 or <1 (-1 for if it can't see the player), then do nothing
        }
    }
    
    private void PerformMeleeAttack(){
        //unimplemented currently
        Debug.Log("Boss melee attack!");
    }

    private void PerformAoEAttack(){
        //unimplemented currently
        Debug.Log("Boss AoE attack!");
    }

}
