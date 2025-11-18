using UnityEngine;

public class BasicPlayerMelee : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.15f; // how long the hitbox lasts
    [SerializeField] private float offset = 0.5f;    // distance from player
    [SerializeField] private int damage = 10;
    private Transform player;
    void Start()
    {
        Destroy(gameObject, lifetime); // disappear after short time
    }
    public void Init(Transform playerTransform)
    {
        player = playerTransform;
        transform.position = player.position + new Vector3(offset, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<BasicEnemy>(out var enemy))
        {
            Debug.Log("Melee hit enemy!");
            enemy.TakeDamage(damage);
        }
    }
}
