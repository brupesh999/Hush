using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicEnemy : Enemy
{
    float timekeeper = 0f;//this will count how much time has passed (in s) since enemy has shot a projectile
    
    [Header ("Shooty settings")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private float detectionDistance = 7f;

    [Header("Health Settings")]
    public override float maxHP { get {return 50f;}}

    [Header ("Move-y settings")]
    protected override Vector3 spawnPoint {get {return new Vector3(4, -2, 0);}} //enemy's origin point, movement will center around
    protected override float movementDistance {get {return 2f;}}//how far enemy will move from spawnPoint
    protected override float movementSpeed {get {return 1f;}}

    private Animator animator;
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

        timekeeper += Time.deltaTime;//increase timer every update
        
        //if player is w/in distance, shoot 'em!
        float playerDistance = DetectPlayer();

        if (playerDistance >= 0 && playerDistance <= detectionDistance){

            //if it's been enough time, instantiate a projectile
            if (timekeeper >= shootInterval){
                timekeeper = 0f;
                CreateProjectile(projectile);
            }
        }

        //no player, move back and forth
        else{
            Move(Time.deltaTime);
        }
    }
}
