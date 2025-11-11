using UnityEngine;

public class BasicEnemyProjectile : MonoBehaviour
{

    [SerializeField] private float velocity = -5f;

    // void Awake(){
    //     renderer = GetComponent<Renderer>();
    // }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * velocity * Time.deltaTime);

        if (!GetComponent<Renderer>().isVisible){
            //if the projectile is no longer on-screen, destroy it
            Destroy(gameObject);
        }
    }
}
