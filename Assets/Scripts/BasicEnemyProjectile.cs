using UnityEngine;

public class BasicEnemyProjectile : MonoBehaviour
{

    [SerializeField] private float velocity = -5f;
    [SerializeField] private int damage = 20;

    // void Awake(){
    //     renderer = GetComponent<Renderer>();
    // }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * velocity * Time.deltaTime);

        if (!GetComponent<Renderer>().isVisible)
        {
            //if the projectile is no longer on-screen, destroy it
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // shield hit
        if (other.TryGetComponent<Shield>(out var shield))
        {
            Debug.Log("Projectile hit shield");
            shield.AbsorbDamage(damage);
            Destroy(gameObject);
            return;
        }

        // player hit
        if (other.TryGetComponent<Player>(out var player))
        {   
            Debug.Log("Projectile hit player");
            player.ApplyDamage(damage);
            Destroy(gameObject);
        }
    }

}
