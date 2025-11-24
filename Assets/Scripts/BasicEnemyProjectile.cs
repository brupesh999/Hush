using UnityEngine;

public class BasicEnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    public float damage = 10f; //needs to be public so hit player can access it
    public Vector3 direction = new Vector3 (-1, 0, 0);//player also needs to access this
    private bool isDeflected = false;

    void Awake(){
        //when instantiated, grab current direction of the enemy which spawned it (parent)
        GameObject ParentEnemy = transform.parent.gameObject;
        direction = ParentEnemy.GetComponent<BasicEnemy>().currentDirection;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

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
            // Debug.Log("Projectile hit shield");
            shield.AbsorbDamage(damage);
            Destroy(gameObject);
            return;
        }

        // player hit
        if (other.TryGetComponent<Player>(out var player))
        {   
            // Debug.Log("Projectile hit player for "+damage);
            player.ApplyDamage(damage);
            Destroy(gameObject);
        }

        // enemy hit
        if (other.TryGetComponent<BasicEnemy>(out var enemy)){
            if (isDeflected){
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }

    public void OnDeflected(){
        isDeflected = true;

        //flip direction
        direction *= -1;

        //halve damage
        damage *= 0.5f;
    }

}
