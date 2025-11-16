using UnityEngine;

public class BasicEnemyProjectile : MonoBehaviour
{

    [SerializeField] private float speed = 5f;
    public float damage = 1f;//needs to be public so hit player can access it
    private Vector3 direction = new Vector3 (-1, 0, 0);

    void Awake(){
        //when instantiated, grab current direction of the enemy which spawned it (parent)
        GameObject ParentEnemy = transform.parent.gameObject;
        direction = ParentEnemy.GetComponent<BasicEnemy>().currentDirection;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if (!GetComponent<Renderer>().isVisible){
            //if the projectile is no longer on-screen, destroy it
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other){
        //when it hits the player, disappear
        if (other.gameObject.tag == "Player"){
            Destroy(gameObject);
        }
    }
}
