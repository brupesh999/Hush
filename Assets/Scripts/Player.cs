using UnityEngine;

public class Player : MonoBehaviour
{
    float attackTimer = 0f; // attack cooldown timer
    [SerializeField] private float attackCooldown = 0.1f; // attack cooldown amount

    // [SerializeField] private float shieldDuration = 1.0f; // how long shield lasts? nvm cus lasts until hit
    // thoughts on small shield cooldown after it breaks/limit on how long u can have shield?
    [SerializeField] private GameObject shieldObject; // shield prefab
    private GameObject activeShield; // shield game object
    private bool shieldActive = false; // shield active or not
    [SerializeField] private GameObject projectile; // projectile prefab
    private GameObject attackProjectile; // active projectile
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        // shieldTimer += Time.deltaTime;

        // attack on press key A
        if (Input.GetKeyDown(KeyCode.A) && attackTimer >= attackCooldown)
        {
            attackTimer = 0f;
            Instantiate(attackProjectile, transform.position + new Vector3(0.5f, 0, 0), Quaternion.identity);
        }

        // shield on press key Q
        if (Input.GetKeyDown(KeyCode.Q) && !shieldActive)
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
