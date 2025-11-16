using UnityEngine;

public class BasicEnemyProjectile : MonoBehaviour
{

    [SerializeField] private float velocity = -5f;
    public float damage = 1f;//needs to be public so hit player can access it

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * velocity * Time.deltaTime);

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
