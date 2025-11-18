using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHP = 100;
    private int currentHP;

    [Header("Attack Settings")]
    [SerializeField] private GameObject meleeAttack;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float attackCooldown = 0.1f;
    private float attackTimer;

    [Header("Shield Settings")]
    [SerializeField] private GameObject shieldObject;
    private GameObject activeShield;
    private bool shieldActive = false;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isGrounded = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
    }

     void Update()
    {
        attackTimer += Time.deltaTime;
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        // shieldTimer += Time.deltaTime;

    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        // Debug.Log("Move input: " + moveInput);
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
        if (context.started && attackTimer >= attackCooldown)
        {
            attackTimer = 0f;

            bool useMelee = true; // rn only melee cus short range lol
            // technically the projectile code works i j havent added long range button
            // should be easy to add??

            if (useMelee)
            {
                GameObject attack = Instantiate(meleeAttack, transform.position, Quaternion.identity);
                attack.GetComponent<BasicPlayerMelee>().Init(transform);
            }
            else
            {
                Instantiate(projectile, transform.position + new Vector3(0.5f, 0, 0), Quaternion.identity);
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
        activeShield = Instantiate(shieldObject, transform.position, Quaternion.identity, transform);
        var shield = activeShield.GetComponent<Shield>();
        if (shield) shield.Init(this);
    }

    public void ApplyDamage(int dmg)
    {
        
        currentHP -= dmg;
        if (currentHP < 0) currentHP = 0;

        Debug.Log("Player took " + dmg + " new Player HP: " + currentHP);
        // j so we know it workslol

        if (currentHP <= 0)
        {
            Debug.Log("Player has died.");
            // need to do some death animation or sth idk
        }
    }

    public void OnPlayerHit(int dmg)
    {
        ApplyDamage(10); // how much damage idk
    }


    public void ShieldBroken()
    {
        shieldActive = false;
        activeShield = null;
        // shieldTimer = 0f;
    }
}
