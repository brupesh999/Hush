//using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHP = 100f;
    public float currentHP;

    [Header("Attack Settings")]
    [SerializeField]
    private GameObject meleeAttack;

    [SerializeField]
    private float meleeDamage = 10f;

    [SerializeField]
    private float attackCooldown = 1f;
    private float attackTimer;

    [SerializeField]
    private int strMultiplier = 2;

    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    private float lrDamage = 5f;

    [SerializeField]
    private float lrCooldown = 5f;
    private float lrTimer;

    [SerializeField]
    private float strCooldown = 4f;
    private float strTimer;

    [SerializeField]
    private float strLRCooldown = 7f;
    private float strLRTimer;

    [Header("Deflect Settings")]
    [SerializeField] private float deflectDistance = 0.5f;
    private List<RaycastHit2D> castResult = new List<RaycastHit2D>();//need this for cast

    [Header("Heal Settings")]
    public int healAmt = 5;
    private float healCooldown = 3f;
    private float healTimer;

    [Header("Shield Settings")]
    [SerializeField]
    private GameObject shieldObject;
    private GameObject activeShield;
    private bool shieldActive = false;

    [Header("Movement Settings")]
    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private float jumpForce = 7f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isGrounded = true;

    [Header("Audio Settings")]
    [SerializeField]
    private AudioSource footsteps;

    [SerializeField]
    private AudioSource hitSound;

    [SerializeField]
    private AudioClip hitSoundClip;

    [Header("Animation Settings")]
    [SerializeField]
    private PlayerAnimationController animationController;

    private float dir;
    private Vector3 fireDirection;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;

        // can use all abilities at the beginning
        attackTimer = attackCooldown;
        healTimer = healCooldown;
        lrTimer = lrCooldown;
        strTimer = strCooldown;
        strLRTimer += strLRCooldown;
    }

    void Update()
    {
        // update ALL cooldown timers
        attackTimer += Time.deltaTime;
        healTimer += Time.deltaTime;
        lrTimer += Time.deltaTime;
        strTimer += Time.deltaTime;
        strLRTimer += Time.deltaTime;

        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        // change direction player is facing (litearlly just flips the sprite)
        if (moveInput.x > 0 && transform.localScale.x < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
        else if (moveInput.x < 0 && transform.localScale.x > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        // player is walking on ground if there is forward movement but no upward movement
        if (moveInput.x != 0 && rb.linearVelocity.y == 0)
        {
            animationController.setIsWalking(true);
            if (!footsteps.isPlaying)
                footsteps.Play();
        }
        else
        {
            animationController.setIsWalking(false);
            if (footsteps.isPlaying)
                footsteps.Stop();
        }

        dir = transform.localScale.x > 0 ? 1 : -1;
        fireDirection = new Vector3(dir, 0, 0);
        // shieldTimer += Time.deltaTime;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        // UnityEngine.Debug.Log("Move input: " + moveInput);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.contacts.Length > 0 && other.contacts[0].normal.y > 0.5f)
            isGrounded = true;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        // UnityEngine.Debug.Log("att timer " + attTimer);
        if (context.started && attackTimer >= attackCooldown)
        {
            attackTimer = 0f;

            animationController.playAttackAnim();
            PlayChordAttack(true, false, false);

            GameObject attack = Instantiate(
                meleeAttack,
                transform.position,
                Quaternion.identity
            );
            attack.GetComponent<BasicPlayerMelee>().Init(transform, meleeDamage, fireDirection);
        }
    }
    public void OnLR(InputAction.CallbackContext context)
    {
        // UnityEngine.Debug.Log("lr timer " + lrTimer);
        if (context.started && lrTimer >= lrCooldown)
        {
            // UnityEngine.Debug.Log("long range started");
            lrTimer = 0f;

            float dir = transform.localScale.x > 0 ? 1 : -1;
            Vector3 fireDirection = new Vector3(dir, 0, 0);
            animationController.playAttackAnim();
            PlayChordAttack(true, false, true);

            GameObject lr = Instantiate(
                projectile,
                transform.position + fireDirection * 1.2f,
                Quaternion.identity
            );

            lr.GetComponent<BasicPlayerProjectile>().Init(transform, lrDamage, fireDirection);
        }
    }
    public void OnStr(InputAction.CallbackContext context)
    {
        UnityEngine.Debug.Log("str timer " + strTimer);
        if (context.started && strTimer >= strCooldown)
        {
            animationController.playAttackAnim();
            PlayChordAttack(true, true, false);

            GameObject attack = Instantiate(
                meleeAttack,
                transform.position,
                Quaternion.identity
            );
            attack.GetComponent<BasicPlayerMelee>().Init(transform, strMultiplier*meleeDamage, fireDirection);
            
        }
    }

    public void OnStrLR(InputAction.CallbackContext context)
    {
        // UnityEngine.Debug.Log("str lr timer " + lrTimer);
        if (context.started && strLRTimer >= strLRCooldown)
        {
            // UnityEngine.Debug.Log("long range started");
            lrTimer = 0f;

            float dir = transform.localScale.x > 0 ? 1 : -1;
            Vector3 fireDirection = new Vector3(dir, 0, 0);
            animationController.playAttackAnim();
            PlayChordAttack(true, true, true);

            GameObject lr = Instantiate(
                projectile,
                transform.position + fireDirection * 1.2f,
                Quaternion.identity
            );

            lr.GetComponent<BasicPlayerProjectile>().Init(transform, lrDamage, strMultiplier*fireDirection);
        }
    }

    public void OnDeflect(InputAction.CallbackContext context){

        //use cast to find projectiles w/in the defined distance
        castResult = new List<RaycastHit2D>();

        if (gameObject.GetComponent<Collider2D>().Cast(fireDirection, castResult, deflectDistance) > 0){
            foreach (RaycastHit2D hitItem in castResult){

                if (hitItem.transform.gameObject.tag == "EnemyProjectile"){
                    //make sure projectile is going to hit Player
                    if (hitItem.transform.gameObject.GetComponent<BasicEnemyProjectile>().direction == fireDirection * -1){
                        hitItem.transform.gameObject.GetComponent<BasicEnemyProjectile>().OnDeflected();

                    //and then deal half damage from projectile to player
                    currentHP -= hitItem.transform.gameObject.GetComponent<BasicEnemyProjectile>().damage / 2f;
                    }
                }
            }
        }
    }

    public void OnShield(InputAction.CallbackContext context)
    {
        if (context.started && !shieldActive)
        {
            ActivateShield();
        }
    }

    public void OnHeal(InputAction.CallbackContext context)
    {
        if (context.started && healTimer >= healCooldown)
        {
            healTimer = 0f;

            if (currentHP <= (maxHP - healAmt))
            {
                currentHP += healAmt;
            }
            else
            {
                currentHP = maxHP;
            }

        }
        else
        {
            // UnityEngine.Debug.Log("heal still on cooldown " + (healCooldown - healTimer) + "seconds left");
        } 
    }
    void LogAttackBar()
    {
        if (MusicConductor.Instance != null)
        {
            int bar = MusicConductor.Instance.CurrentBar;

            Debug.Log($"ATTACK on Bar {bar}");
        }
        else
        {
            Debug.Log("MusicConductor not found");
        }
    }

    void PlayChordAttack(bool low, bool mid, bool high)
    {
        if (MusicConductor.Instance != null)
        {
            MusicConductor.Instance.PlayAttackNotes(low, mid, high);
        }
    }


    void ActivateShield()
    {
        shieldActive = true;
        // shieldTimer = 0f;
        activeShield = Instantiate(
            shieldObject,
            transform.position,
            Quaternion.identity,
            transform
        );
        var shield = activeShield.GetComponent<Shield>();
        if (shield)
            shield.Init(this);
    }

    public void ApplyDamage(float dmg)
    {
        // UnityEngine.Debug.Log("ApplyDamage dmg is "+dmg);
        //Debug.Log("ApplyDamage called");//DEBUG
        currentHP -= dmg;
        if (currentHP < 0) currentHP = 0;

        // UnityEngine.Debug.Log("Player took " + dmg + " new Player HP: " + currentHP);
        // j so we know it workslol
        if (shieldActive)
        {
            hitSound.volume = 0.25f;
        } else
        {
            hitSound.volume = 1.0f;
        }
        hitSound.PlayOneShot(hitSoundClip);

        if (currentHP <= 0)
        {
            animationController.playDeathAnim();
            // UnityEngine.Debug.Log("Player has died.");
            // need to do some death animation or sth idk
        } else
        {
            animationController.playHitAnim();
        }
        // Debug.Log("Player HP is "+currentHP);//DEBUG - for some reason this wasn't printing but the earlier one was??
    }

    // public void OnPlayerHit(int dmg)
    // {
    //     ApplyDamage(10); // how much damage idk
    // }


    public void ShieldBroken()
    {
        shieldActive = false;
        activeShield = null;
        // shieldTimer = 0f;
    }
}
