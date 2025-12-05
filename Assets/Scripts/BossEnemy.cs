using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BossEnemy : Enemy
{
    float timekeeper = 0f;//how much time has passed since performing any attack
    float projectileTimekeeper = 0f;//this will count how much time has passed (in s) since enemy has shot a projectile
    float meleeTimekeeper = 0f;//how much time has passed (in s) since enemy has performed melee attack
    float AoETimekeeper = 0f;//same for AoE attack

    [Header ("Prefabs")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject AoEAttack;

    [Header ("Cooldowns")]
    [SerializeField] private float AoECooldown = 5f;
    [SerializeField] private float projectileCooldown = 2f;
    [SerializeField] private float meleeCooldown = 1.5f;
    [SerializeField] private float overallAttackCooldown = 1f;

    [Header ("Distances for attacks")]
    [SerializeField] private float AoEDetectionDistance = 10f;
    [SerializeField] private float projectileDetectionDistance = 7f;
    [SerializeField] private float meleeDetectionDistance = 3f;

    [Header ("Damage & Health")]
    [SerializeField] private float meleeAttackDamage = 30f;
    public override float maxHP { get {return 300f;}}

    [Header ("Position settings")]
    //current direction and castResult defined in parent class (Enemy)

    [SerializeField] private float groundYPosition = -4f;//DEBUG - should find this in code, but idk how we're doing the ground rn

    // [SerializeField] private Vector3 spawnPoint = new Vector3(4, 0, 0); //enemy's origin point, movement will center around
    protected override float movementDistance {get {return 0f;}}//how far enemy will move from spawnPoint
    protected override float movementSpeed {get {return 0f;}}

    private Animator animator;
    [SerializeField] private float secDeathAnimationLength = 1.4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
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
                    animator.SetTrigger("TriggerAttack");
                    PerformMeleeAttack(meleeDetectionDistance, meleeAttackDamage);
                    //reset CD timer
                    meleeTimekeeper = 0;
                }
            }

            //mid-range (projectile attack) - 1 to 7
            else if (playerDistance <= projectileDetectionDistance){
                if (projectileTimekeeper >= projectileCooldown){
                    timekeeper = 0f;
                    animator.SetTrigger("TriggerAttack");
                    CreateProjectile(projectile);
                    //reset CD timer
                    projectileTimekeeper = 0f;
                }
            }
            
            //ranged (AoE attack) - 7 to 10
            //DEBUG - currently no projectile for ranged
            else if (playerDistance <= AoEDetectionDistance){
                if (AoETimekeeper >= AoECooldown){
                    timekeeper = 0f;
                    animator.SetTrigger("TriggerAttack");
                    PerformAoEAttack();
                    //reset CD timer
                    AoETimekeeper = 0;
                }
            }

            //if >10 or <1 (-1 for if it can't see the player), then do nothing
        }
    }

    private void PerformAoEAttack(){

        CreatePrefab(AoEAttack, new Vector3(player.transform.position.x, groundYPosition, 0));
    }

    public override void Die()
    {
        Debug.Log("Boss Enemy died!");

        if (EnemyManager.Instance != null) {
            EnemyManager.Instance.UnregisterEnemy(this);
        }

        //move child projectiles before destroying
        foreach(Transform child in gameObject.GetComponentsInChildren<Transform>()){
            child.gameObject.transform.SetParent(null);
        }


        StartCoroutine(DeathAnimation());
    }

    private IEnumerator DeathAnimation(){

        animator.SetBool("isDead",true);

        yield return new WaitForSeconds(secDeathAnimationLength);

        Destroy(gameObject);
        Destroy(GameObject.FindWithTag("Player"));
        SceneManager.LoadScene("EndScene", LoadSceneMode.Additive);
    }


}
