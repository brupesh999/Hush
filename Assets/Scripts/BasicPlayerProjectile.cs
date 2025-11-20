using UnityEngine;

public class BasicPlayerProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private int damage;
    private Vector3 direction = new Vector3 (1, 0, 0);
    private Rigidbody2D rb;

    // Update is called once per frame
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Transform playerTransform, int damageAmount, Vector3 directionIn)
    {
        damage = damageAmount;
        direction = directionIn.normalized;

        rb.linearVelocity = direction * speed;

        Destroy(gameObject, 10f);

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<BasicEnemy>(out var enemy))
        {
            Debug.Log("Projectile hit enemy!");
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
