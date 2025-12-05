using UnityEngine;

public class BasicPlayerProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private float damage;
    private Vector3 direction = new Vector3 (1, 0, 0);
    private Rigidbody2D rb;

    private Animator animator;

    // Update is called once per frame
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); 

    }

    public void Init(Transform playerTransform, float damageAmount, Vector3 directionIn)
    {
        damage = damageAmount;
        direction = directionIn.normalized;

        rb.linearVelocity = direction * speed;

        Destroy(gameObject, 10f);

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out var enemy))
        {
            Debug.Log("Projectile hit enemy!");
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    public void playLRAnim(Vector2 playerPos)
    {
        // Debug.Log("orange");
        float additionalOffset = direction[0] == 1 ? 0.5f : 0.0f;
        transform.position = playerPos + new Vector2(1.5f * direction[0] + additionalOffset, 0f);
        animator.SetTrigger("launchOrangeAttack");
    }

    public void playStrengthLRAnim(Vector2 playerPos)
    {
        // Debug.Log("yellow");
        float additionalOffset = direction[0] == 1 ? 0.5f : 0.0f;
        transform.position = playerPos + new Vector2(1.5f * direction[0] + additionalOffset, 0f);
        animator.SetTrigger("launchYellowAttack");
    }
}
