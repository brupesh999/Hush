using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    float timekeeper = 0f;//this will count how much time has passed (in s) since enemy has shot a projectile
    [SerializeField] private GameObject projectile;
    [SerializeField] private float shootInterval;

    [Header("Health Settings")]
    [SerializeField] private int maxHP = 50;
    private int currentHP;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        timekeeper += Time.deltaTime;//increase timer every update

        if (timekeeper >= shootInterval)
        {
            timekeeper = 0f;
            Instantiate(projectile, transform.position + new Vector3(-0.5f, 0, 0), Quaternion.identity);
        }
    }
    
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log("Enemy took " + damage + " damage. HP: " + currentHP);
        
        if (currentHP <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }
}
