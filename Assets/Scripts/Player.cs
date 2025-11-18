using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField]
    private GameObject meleeAttack;

    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    private float attackCooldown = 0.1f;
    private float attackTimer;

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

    [Header("Player Settings")]
    public float currentHP = 10f; //needs to be public so music can access it
    public float maxHP = 10f; //needs to be public so music can access it

    [Header("Audio Settings")]
    [SerializeField]
    private AudioSource footsteps;

    [SerializeField]
    private AudioSource hitSound;

    [SerializeField]
    private AudioClip hitSoundClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        attackTimer += Time.deltaTime;
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        // player is walking on ground if there is forward movement but no upward movement
        if (moveInput.x != 0 && rb.linearVelocity.y == 0)
        {
            if (!footsteps.isPlaying)
                footsteps.Play();
        }
        else
        {
            if (footsteps.isPlaying)
                footsteps.Stop();
        }
        // shieldTimer += Time.deltaTime;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log("Move input: " + moveInput);
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

        //when hit by enemy projectile, subtract damage from player HP
        if (other.gameObject.tag == "EnemyProjectile")
        {
            currentHP -= other.gameObject.GetComponent<BasicEnemyProjectile>().damage;
            hitSound.PlayOneShot(hitSoundClip);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && attackTimer >= attackCooldown)
        {
            attackTimer = 0f;

            bool useMelee = true; // rn only melee cus short range lol
            // technically the projectile code works i j havent added long range button
            // should be easy to add??

            if (useMelee)
            {
                GameObject attack = Instantiate(
                    meleeAttack,
                    transform.position,
                    Quaternion.identity
                );
                attack.GetComponent<BasicPlayerMelee>().Init(transform);
            }
            else
            {
                Instantiate(
                    projectile,
                    transform.position + new Vector3(0.5f, 0, 0),
                    Quaternion.identity
                );
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

    public void OnPlayerHit()
    {
        Debug.Log("Player took damage!");
        // eventually add hp reduction
    }

    public void ShieldBroken()
    {
        shieldActive = false;
        activeShield = null;
        // shieldTimer = 0f;
    }
}
