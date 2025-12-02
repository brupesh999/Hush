using UnityEngine;

public class BasicPlayerMelee : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.15f; // how long the hitbox lasts
    [SerializeField] private float offset = 0.5f;    // distance from player
    private float damage;
    private Vector3 direction;

    private Animator animator;
    void Start()
    {
        Destroy(gameObject, lifetime); // disappear after short time
    }
    public void Init(Transform playerTransform, float damageAmount, Vector3 directionIn)
    {
        animator = GetComponent<Animator>(); 
        damage = damageAmount;
        direction = directionIn.normalized;

        transform.position = playerTransform.position + direction * offset;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<BasicEnemy>(out var enemy))
        {
            // Debug.Log("Melee hit for " + damage + " damage");
            enemy.TakeDamage(damage);
        }
    }

    public void playAttackAnim(Vector2 playerPos)
    {
        Debug.Log("blue");
        transform.position = playerPos + new Vector2(1.5f, 1.0f);
        animator.SetTrigger("launchBlueAttack");
    }

    public void playStrengthAnim(Vector2 playerPos)
    {
        Debug.Log("red");
        transform.position = playerPos + new Vector2(1.5f, 1.0f);
        animator.SetTrigger("launchRedAttack");
    }
}
